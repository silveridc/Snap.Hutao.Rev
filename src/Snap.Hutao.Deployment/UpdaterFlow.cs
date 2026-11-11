// Github:https://github.com/zhaoyj7 All rights reserved.
// Licensed under the MIT license

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snap.Hutao.Deployment;

internal static partial class UpdaterFlow
{
    private const string UserAgent = "Snap Hutao/Deployment";
    private const string PackageName = "zhaoyj.Snap.Hutao.Rev";
    private const string MainProcessName = "Snap.Hutao";
    private const string WorkDirectoryName = "HutaoLauncher.Deployment";
    private const string PackageFileName = "HutaoLauncher.msix";
    private const string ScriptFileName = "install.ps1";

    private static readonly string InstallScript = """
        param(
            [Parameter(Mandatory = $true)][string]$MsixPath,
            [Parameter(Mandatory = $true)][string]$PackageName,
            [Parameter(Mandatory = $true)][string]$ProcessName)

        $ErrorActionPreference = 'Stop'
        Add-AppxPackage -Path $MsixPath -ForceUpdateFromAnyVersion -ForceApplicationShutdown

        for ($i = 0; $i -lt 20; $i++) {
            if (-not (Get-Process -Name $ProcessName -ErrorAction SilentlyContinue)) { break }
            Start-Sleep -Milliseconds 500
        }

        $package = Get-AppxPackage -Name $PackageName
        if ($null -ne $package) {
            $activation = "shell:AppsFolder\$($package.PackageFamilyName)!App"
            for ($i = 0; $i -lt 3; $i++) {
                Start-Process -FilePath 'explorer.exe' -ArgumentList $activation
                Start-Sleep -Seconds 2
                if (Get-Process -Name $ProcessName -ErrorAction SilentlyContinue) { break }
            }
        }
        """;

    internal static async Task RunAsync(IProgress<UpdateState> progress, CancellationToken token)
    {
        progress.Report(new UpdateState(UpdateStage.Connecting, "正在获取版本信息", string.Empty, null));

        ReleaseInfo release = await GetReleaseInfoAsync(token).ConfigureAwait(false);
        Uri downloadUrl = PickDirectMirror(release);

        progress.Report(new UpdateState(UpdateStage.Downloading, "正在下载更新包", $"来自 {downloadUrl.Host}", 0d, true, release.Version, release.ReleaseNotes));

        string packagePath = await DownloadAsync(downloadUrl, progress, token).ConfigureAwait(false);
        try
        {
            progress.Report(new UpdateState(UpdateStage.Verifying, "正在比对服务端发布的校验值", string.Empty, 100d, false));
            VerifySha256(packagePath, release.Validation);

            progress.Report(new UpdateState(UpdateStage.Installing, "安装完成后会自动重新打开胡桃启动器", string.Empty, null, false));
            Install(packagePath);
        }
        finally
        {
            Delete(packagePath);
        }
    }

    private static async Task<ReleaseInfo> GetReleaseInfoAsync(CancellationToken token)
    {
        using HttpClient client = CreateClient();
        string requestUrl = $"{ServerHosts.Current.Api.TrimEnd('/')}/patch/hutao";
        using HttpResponseMessage response = await client.GetAsync(requestUrl, token).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        ReleaseResponse? envelope = JsonSerializer.Deserialize(json, ReleaseJsonContext.Default.ReleaseResponse);

        if (envelope?.Retcode != 0)
        {
            throw new InvalidOperationException($"服务端返回 retcode={envelope?.Retcode} {envelope?.Message}");
        }

        return envelope.Data ?? throw new InvalidOperationException("服务端未发布更新信息");
    }

    private static Uri PickDirectMirror(ReleaseInfo release)
    {
        if (!IsSha256Hex(release.Validation))
        {
            throw new InvalidOperationException("服务端发布的校验值不是 SHA-256");
        }

        foreach (ReleaseMirror mirror in release.Mirrors)
        {
            if (!string.Equals(mirror.MirrorType, "Direct", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (Uri.TryCreate(mirror.Url, UriKind.Absolute, out Uri? url) && url.Scheme == Uri.UriSchemeHttps)
            {
                return url;
            }
        }

        throw new InvalidOperationException("服务端未提供可用的 Direct 镜像地址");
    }

    private static async Task<string> DownloadAsync(Uri downloadUrl, IProgress<UpdateState> progress, CancellationToken token)
    {
        string workDirectory = Path.Combine(Path.GetTempPath(), WorkDirectoryName);
        Directory.CreateDirectory(workDirectory);
        string packagePath = Path.Combine(workDirectory, PackageFileName);

        using HttpClient client = CreateClient();
        using HttpResponseMessage response = await client
            .GetAsync(downloadUrl.ToString(), HttpCompletionOption.ResponseHeadersRead, token)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        long totalBytes = response.Content.Headers.ContentLength ?? -1L;
        Stopwatch stopwatch = Stopwatch.StartNew();
        await using (Stream source = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false))
        await using (FileStream target = new(packagePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            Memory<byte> buffer = new byte[64 * 1024];
            long writtenBytes = 0L;
            int lastShownPercent = -1;
            int readBytes;
            while ((readBytes = await source.ReadAsync(buffer, token).ConfigureAwait(false)) > 0)
            {
                await target.WriteAsync(buffer[..readBytes], token).ConfigureAwait(false);
                writtenBytes += readBytes;

                // 少数镜像分块传输不给 Content-Length，此时只能显示未知进度。
                int percent = totalBytes > 0L ? (int)(writtenBytes * 100L / totalBytes) : -1;
                if (percent == lastShownPercent)
                {
                    continue;
                }

                lastShownPercent = percent;
                double bytesPerSecond = writtenBytes * 1000d / stopwatch.ElapsedMilliseconds;
                string status = totalBytes > 0L
                    ? $"{percent}% · {ToMiB(writtenBytes)} / {ToMiB(totalBytes)} · {ToMiB((long)bytesPerSecond)}/s"
                    : $"{ToMiB(writtenBytes)} · {ToMiB((long)bytesPerSecond)}/s";
                progress.Report(new UpdateState(UpdateStage.Downloading, "正在下载更新包", status, percent < 0 ? null : percent));
            }

            await target.FlushAsync(token).ConfigureAwait(false);
        }

        return packagePath;
    }

    private static void VerifySha256(string packagePath, string validation)
    {
        string actual;
        using (SHA256 sha256 = SHA256.Create())
        using (FileStream file = File.OpenRead(packagePath))
        {
            actual = Convert.ToHexString(sha256.ComputeHash(file));
        }

        if (!string.Equals(actual, validation, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"校验值不匹配，安装包可能已损坏（期望 {ShortHash(validation)}，实际 {ShortHash(actual)}）");
        }
    }

    private static string ShortHash(string value)
    {
        return value.Length <= 12 ? value.ToLowerInvariant() : $"{value[..12].ToLowerInvariant()}…";
    }

    private static void Install(string packagePath)
    {
        string workDirectory = Path.GetDirectoryName(packagePath)!;
        string scriptPath = Path.Combine(workDirectory, ScriptFileName);

        // 带 BOM 写出：Windows PowerShell 5.1 读无 BOM 的 UTF-8 脚本会按 ANSI 解码，脚本里的中文注释会连带把语法吃掉。
        File.WriteAllText(scriptPath, InstallScript, Encoding.UTF8);

        try
        {
            using Process process = new()
            {
                StartInfo = new ProcessStartInfo("powershell.exe")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,

                    // 更新器是 GUI 进程，没有可继承的控制台；不重定向的话 powershell 会在写输出时卡住。
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    ArgumentList =
                    {
                        "-NoProfile",
                        "-ExecutionPolicy",
                        "Bypass",
                        "-File",
                        scriptPath,
                        "-MsixPath",
                        packagePath,
                        "-PackageName",
                        PackageName,
                        "-ProcessName",
                        MainProcessName,
                    },
                },
            };

            StringBuilder log = new();
            void Append(object sender, DataReceivedEventArgs e)
            {
                if (e.Data is not null)
                {
                    lock (log)
                    {
                        _ = log.AppendLine(e.Data);
                    }
                }
            }

            process.OutputDataReceived += Append;
            process.ErrorDataReceived += Append;
            if (!process.Start())
            {
                throw new InvalidOperationException("无法启动 powershell.exe");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                string tail = log.ToString().Trim();
                int from = Math.Max(0, tail.Length - 400);
                throw new InvalidOperationException($"{DescribeInstallFailure(tail)}安装失败（退出码 {process.ExitCode}）{tail[from..]}");
            }
        }
        finally
        {
            Delete(scriptPath);
        }
    }

    private static string DescribeInstallFailure(string log)
    {
        // 0x80073CFB：Windows 不允许安装包覆盖“松散注册”（Add-AppxPackage -Register）的同标识包，
        // 开发机上很常见，给用户一个能自己处理的提示
        if (log.Contains("0x80073CFB", StringComparison.OrdinalIgnoreCase)
            || log.Contains("unpackaged", StringComparison.OrdinalIgnoreCase)
            || log.Contains("未打包", StringComparison.Ordinal))
        {
            return "当前用户安装的是开发者松散版本（目录注册），安装包无法覆盖；请先卸载旧版本再更新。";
        }

        return string.Empty;
    }

    private static HttpClient CreateClient()
    {
        HttpClient client = new()
        {
            Timeout = TimeSpan.FromMinutes(5),
        };

        client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        return client;
    }

    private static bool IsSha256Hex(string validation)
    {
        if (validation.Length != 64)
        {
            return false;
        }

        foreach (char c in validation)
        {
            if (!Uri.IsHexDigit(c))
            {
                return false;
            }
        }

        return true;
    }

    private static string ToMiB(long bytes)
    {
        return $"{bytes / 1048576d:F1} MiB";
    }

    private static void Delete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            // 临时文件删不掉不影响更新结果。
        }
    }

    [JsonSerializable(typeof(ReleaseResponse))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
    private sealed partial class ReleaseJsonContext : JsonSerializerContext;

    private sealed class ReleaseResponse
    {
        public int Retcode { get; set; }

        public string? Message { get; set; }

        public ReleaseInfo? Data { get; set; }
    }

    private sealed class ReleaseInfo
    {
        public string Version { get; set; } = string.Empty;

        public string Validation { get; set; } = string.Empty;

        public List<ReleaseMirror> Mirrors { get; set; } = [];

        // 服务端下发的是 camelCase，这里显式覆盖 SnakeCaseLower 命名策略
        [JsonPropertyName("releaseNotes")]
        public string? ReleaseNotes { get; set; }
    }

    private sealed class ReleaseMirror
    {
        public string Url { get; set; } = string.Empty;

        public string MirrorName { get; set; } = string.Empty;

        public string MirrorType { get; set; } = string.Empty;
    }
}

internal enum UpdateStage
{
    Connecting,
    Downloading,
    Verifying,
    Installing,
    Completed,
    Canceled,
    Failed,
}

internal readonly record struct UpdateState(
    UpdateStage Stage,
    string Detail,
    string Status,
    double? Percent,
    bool Cancellable = true,
    string? Version = null,
    string? ReleaseNotes = null);
