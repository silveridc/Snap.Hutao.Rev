// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Caching.Memory;
using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Core.IO.Hashing;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Collections.Immutable;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Snap.Hutao.Rev.Service.Metadata;

[Service(ServiceLifetime.Singleton, typeof(IMetadataService))]
[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class MetadataService : IMetadataService
{
    private readonly TaskCompletionSource initializeCompletionSource = new();

    private readonly IHutaoEndpointsFactory hutaoEndpointsFactory;
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    private readonly ILogger<MetadataService> logger;
    private readonly MetadataOptions metadataOptions;
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions options;

    private volatile bool isInitialized;

    // 远端 Meta.json 校验和清单(fileName -> XxHash64);离线取不到时为 null,退回本地缓存
    private Dictionary<string, string>? remoteHashes;

    [GeneratedConstructor]
    public partial MetadataService(IServiceProvider serviceProvider);

    public partial IMemoryCache MemoryCache { get; }

    public async ValueTask<bool> InitializeAsync()
    {
        await initializeCompletionSource.Task.ConfigureAwait(false);
        return isInitialized;
    }

    public async ValueTask InitializeInternalAsync(CancellationToken token = default)
    {
        if (isInitialized)
        {
            return;
        }

        // 拉取远端 Meta.json 校验和清单(仅一个小文件);拿不到(如离线)则退回使用本地缓存
        remoteHashes = await TryGetRemoteHashesAsync(token).ConfigureAwait(false);
        isInitialized = true;
        initializeCompletionSource.TrySetResult();
    }

    private async ValueTask<Dictionary<string, string>?> TryGetRemoteHashesAsync(CancellationToken token)
    {
        // logException:false —— 拿不到 Meta.json(如离线)时静默退回本地缓存,不弹错误提示
        string metaUri = hutaoEndpointsFactory.Create().MetadataStatic(metadataOptions.LocaleName, "Meta");
        Dictionary<string, string>? hashes = await httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(metaUri)
            .Get()
            .SendAsync<Dictionary<string, string>>(httpClient, false, token)
            .ConfigureAwait(false);

        if (hashes is null)
        {
            logger.LogWarning("Failed to fetch metadata Meta.json, fall back to local cache");
        }

        return hashes;
    }

    public async ValueTask<ImmutableArray<T>> FromCacheOrFileAsync<T>(MetadataFileStrategy strategy, CancellationToken token)
        where T : class
    {
        HutaoException.ThrowIfNot(isInitialized, SH.ServiceMetadataNotInitialized);
        string cacheKey = $"{nameof(MetadataService)}.Cache.{strategy.Name}";

        if (MemoryCache.TryGetValue(cacheKey, out object? value))
        {
            ArgumentNullException.ThrowIfNull(value);
            return Unsafe.Unbox<ImmutableArray<T>>(value);
        }

        // 按 Meta.json 校验本地文件,只增量下载缺失/变更的并落盘
        if (remoteHashes is { } hashes)
        {
            await EnsureStrategyFilesAsync(strategy, hashes, token).ConfigureAwait(false);
        }

        string localPath = metadataOptions.GetLocalizedLocalPath(strategy.IsScattered ? strategy.Name : $"{strategy.Name}.json");

        if (strategy.IsScattered)
        {
            if (Directory.Exists(localPath) && Directory.GetFiles(localPath, "*.json").Length > 0)
            {
                return FromCacheOrScatteredFile<T>(strategy, cacheKey, localPath);
            }
        }
        else if (File.Exists(localPath))
        {
            return FromCacheOrSingleFile<T>(strategy, cacheKey, localPath);
        }

        return await DownloadMetadataAsync<T>(strategy, cacheKey, token).ConfigureAwait(false);
    }

    private ImmutableArray<T> FromCacheOrSingleFile<T>(MetadataFileStrategy strategy, string cacheKey, string path)
        where T : class
    {
        using (Stream fileStream = File.OpenRead(path))
        {
            try
            {
                ImmutableArray<T> result = JsonSerializer.Deserialize<ImmutableArray<T>>(fileStream, options);
                return MemoryCache.Set(cacheKey, result);
            }
            catch (Exception ex)
            {
                ex.Data.Add("FileName", strategy.Name);
                throw;
            }
        }
    }

    private ImmutableArray<T> FromCacheOrScatteredFile<T>(MetadataFileStrategy strategy, string cacheKey, string path)
        where T : class
    {
        ImmutableArray<T>.Builder results = ImmutableArray.CreateBuilder<T>();
        foreach (string file in Directory.GetFiles(path, "*.json"))
        {
            string fileName = $"{strategy.Name}/{Path.GetFileNameWithoutExtension(file)}";
            if (remoteHashes is not null && !remoteHashes.ContainsKey(fileName))
            {
                // 清单里已不存在的散文件(如被移除的角色),跳过不加载
                continue;
            }

            using (Stream fileStream = File.OpenRead(file))
            {
                try
                {
                    T? result = JsonSerializer.Deserialize<T>(fileStream, options);
                    ArgumentNullException.ThrowIfNull(result);
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("FileName", fileName);
                    throw;
                }
            }
        }

        return MemoryCache.Set(cacheKey, results.ToImmutable());
    }

    private async ValueTask EnsureStrategyFilesAsync(MetadataFileStrategy strategy, Dictionary<string, string> hashes, CancellationToken token)
    {
        if (strategy.IsScattered)
        {
            string prefix = $"{strategy.Name}/";
            await Parallel.ForEachAsync(hashes.Where(kvp => kvp.Key.StartsWith(prefix, StringComparison.Ordinal)), token, async (pair, ct) =>
            {
                await EnsureFileAsync(pair.Key, pair.Value, ct).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        else if (hashes.TryGetValue(strategy.Name, out string? hash))
        {
            await EnsureFileAsync(strategy.Name, hash, token).ConfigureAwait(false);
        }
    }

    private async ValueTask EnsureFileAsync(string fileName, string metaHash, CancellationToken token)
    {
        string localPath = metadataOptions.GetLocalizedLocalPath($"{fileName}.json");
        if (Path.GetDirectoryName(localPath) is { } directory && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (await IsLocalFileValidAsync(localPath, metaHash, token).ConfigureAwait(false))
        {
            return;
        }

        await DownloadFileToDiskAsync(fileName, localPath, token).ConfigureAwait(false);
    }

    private static async ValueTask<bool> IsLocalFileValidAsync(string path, string metaHash, CancellationToken token)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            // 用「行尾归一化(CRLF→LF)后」的哈希比对:这样本地文件无论是 CRLF 还是 LF,
            // 都能与 LF 版 Meta.json 的哈希匹配,不再挑行尾。
            string fileHash = await HashFileNormalizedAsync(path, token).ConfigureAwait(false);
            return string.Equals(metaHash, fileHash, StringComparison.OrdinalIgnoreCase);
        }
        catch (IOException)
        {
            return false;
        }
    }

    private static async ValueTask<string> HashFileNormalizedAsync(string path, CancellationToken token)
    {
        byte[] bytes = await File.ReadAllBytesAsync(path, token).ConfigureAwait(false);

        // 就地去掉所有 CR(0x0D),把 CRLF 归一化成 LF 再算哈希(JSON 里的裸 0x0D 只会是行尾)
        int write = 0;
        for (int read = 0; read < bytes.Length; read++)
        {
            if (bytes[read] != (byte)'\r')
            {
                bytes[write++] = bytes[read];
            }
        }

        using (MemoryStream stream = new(bytes, 0, write))
        {
            return await XxHash64.HashAsync(stream, token).ConfigureAwait(false);
        }
    }

    private async ValueTask DownloadFileToDiskAsync(string fileName, string localPath, CancellationToken token)
    {
        string requestUri = hutaoEndpointsFactory.Create().MetadataStatic(metadataOptions.LocaleName, fileName);
        string tempPath = $"{localPath}.tmp";
        using (HttpResponseMessage response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
        {
            response.EnsureSuccessStatusCode();

            // 原样写盘,不改行尾:静态站发的是 LF,Meta.json 的哈希也按原始字节(LF)计算,
            // 任何转换都会让 XxHash64 永远对不上、每次启动全量重下。先写临时文件再原子替换,避免中断留下损坏文件。
            Stream sourceStream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
            using (FileStream fileStream = File.Create(tempPath))
            {
                await sourceStream.CopyToAsync(fileStream, token).ConfigureAwait(false);
            }
        }

        try
        {
            File.Move(tempPath, localPath, overwrite: true);
        }
        catch (UnauthorizedAccessException)
        {
            // 覆盖被拒多为残留的只读/被占用旧文件,清掉再移一次
            File.Delete(localPath);
            File.Move(tempPath, localPath, overwrite: true);
        }
    }

    private async ValueTask<ImmutableArray<T>> DownloadMetadataAsync<T>(MetadataFileStrategy strategy, string cacheKey, CancellationToken token)
        where T : class
    {
        // 分散型元数据(如 Avatar)在仓库里是 {Name}/{id}.json,没有聚合文件,需靠 Meta.json 枚举后逐个下载
        if (strategy.IsScattered)
        {
            return await DownloadScatteredMetadataAsync<T>(strategy, cacheKey, token).ConfigureAwait(false);
        }

        string requestUri = hutaoEndpointsFactory.Create().MetadataStatic(metadataOptions.LocaleName, strategy.Name);
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(requestUri)
            .Get();

        List<T>? response = await builder.SendAsync<List<T>>(httpClient, token).ConfigureAwait(false);
        HutaoException.ThrowIf(response is null, SH.FormatWebResponseRequestException(requestUri, nameof(DownloadMetadataAsync)));
        return MemoryCache.Set(cacheKey, response.ToImmutableArray());
    }

    private async ValueTask<ImmutableArray<T>> DownloadScatteredMetadataAsync<T>(MetadataFileStrategy strategy, string cacheKey, CancellationToken token)
        where T : class
    {
        string metaUri = hutaoEndpointsFactory.Create().MetadataStatic(metadataOptions.LocaleName, "Meta");
        Dictionary<string, string>? meta = await httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(metaUri)
            .Get()
            .SendAsync<Dictionary<string, string>>(httpClient, token)
            .ConfigureAwait(false);
        HutaoException.ThrowIf(meta is null, SH.FormatWebResponseRequestException(metaUri, nameof(DownloadScatteredMetadataAsync)));

        string prefix = $"{strategy.Name}/";
        ImmutableArray<T>.Builder results = ImmutableArray.CreateBuilder<T>();
        foreach (string key in meta.Keys)
        {
            if (!key.StartsWith(prefix, StringComparison.Ordinal))
            {
                continue;
            }

            string itemUri = hutaoEndpointsFactory.Create().MetadataStatic(metadataOptions.LocaleName, key);
            T? item = await httpRequestMessageBuilderFactory.Create()
                .SetRequestUri(itemUri)
                .Get()
                .SendAsync<T>(httpClient, token)
                .ConfigureAwait(false);
            HutaoException.ThrowIf(item is null, SH.FormatWebResponseRequestException(itemUri, nameof(DownloadScatteredMetadataAsync)));
            results.Add(item);
        }

        return MemoryCache.Set(cacheKey, results.ToImmutable());
    }
}
