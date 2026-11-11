// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.Core.ExceptionService;
using Snap.Hutao.Rev.Core.IO;
using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Factory.Process;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Win32;
using System.Collections.Frozen;
using System.IO;
using ThemeFile = (Microsoft.UI.Xaml.ElementTheme, Snap.Hutao.Rev.Core.IO.ValueFile);

namespace Snap.Hutao.Rev.Core.Caching;

[Service(ServiceLifetime.Singleton, typeof(IImageCache))]
internal sealed partial class ImageCache : IImageCache
{
    private static readonly FrozenSet<string> SupportedSchemes =
    [
        Uri.UriSchemeHttp,
        Uri.UriSchemeHttps,
    ];

    private readonly AsyncKeyedLock<ThemeFile> themeFileLocks = new();
    private readonly AsyncKeyedLock<string> downloadLocks = new();

    private readonly IImageCacheDownloadOperation downloadOperation;

    [GeneratedConstructor]
    public partial ImageCache(IServiceProvider serviceProvider);

    private string CacheFolder
    {
        get => LazyInitializer.EnsureInitialized(ref field, static () =>
        {
            try
            {
                string folder = HutaoRuntime.GetLocalCacheImageCacheDirectory();
                Directory.CreateDirectory(Path.Combine(folder, "Light"));
                Directory.CreateDirectory(Path.Combine(folder, "Dark"));
                return folder;
            }
            catch (Exception ex)
            {
                // 0x80070570 ERROR_FILE_CORRUPT
                NativeSystem.ShowErrorMessage(ex.Message, ExceptionFormat.Format(ex));
                ProcessFactory.KillCurrent();
                return string.Empty;
            }
        });
    }

    public void Remove(Uri uriForCachedItem)
    {
        try
        {
            File.Delete(ImageCacheFile.GetHashedFile(CacheFolder, uriForCachedItem));
        }
        catch
        {
            // Ignored
        }
    }

    public async ValueTask<ValueFile> GetFileFromCacheAsync(Uri uri)
    {
        return await GetFileFromCacheAsync(uri, ElementTheme.Default).ConfigureAwait(false);
    }

    public async ValueTask<ValueFile> GetFileFromCacheAsync(Uri uri, ElementTheme theme)
    {
        // Debug.Assert 在 Release 里会被剥掉，非 http(s) 的 URI（例如 file:/// 本地路径）必须在这里挡住，
        // 否则会被当成普通 URL 哈希成缓存键再去下载/解码
        if (!SupportedSchemes.Contains(uri.Scheme))
        {
            InternalImageCacheException.Throw($"Unsupported image URI scheme: '{uri}'");
        }

        ImageCacheFile imageCacheFile = ImageCacheFile.Create(CacheFolder, uri);
        string themedFileFullPath = imageCacheFile.GetThemedFile(theme);

        using (await themeFileLocks.LockAsync((theme, imageCacheFile.HashedFileName)).ConfigureAwait(false))
        {
            if (IsFileInvalid(themedFileFullPath))
            {
                using (await downloadLocks.LockAsync(imageCacheFile.HashedFileName).ConfigureAwait(false))
                {
                    if (IsFileInvalid(imageCacheFile.DefaultFilePath) || !ImageFileSignature.IsValidFile(imageCacheFile.DefaultFilePath))
                    {
                        SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateInfo("Begin to download file", "Core.Caching.ImageCache", [("Uri", uri.ToString()), ("File", imageCacheFile.DefaultFilePath)]));

                        try
                        {
                            await downloadOperation.DownloadFileAsync(uri, imageCacheFile.DefaultFilePath).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                            Remove(uri);
                            throw;
                        }
                    }
                }
            }

            await EnsureThemedMonochromeFileExistsAsync(imageCacheFile, theme).ConfigureAwait(false);
            return themedFileFullPath;
        }
    }

    public ValueFile GetFileFromCategoryAndName(string category, string fileName)
    {
        return ImageCacheFile.Create(CacheFolder, StaticResourcesEndpoints.StaticRaw(category, fileName)).DefaultFilePath;
    }

    private static bool IsFileInvalid(string file, bool treatNullFileAsInvalid = true)
    {
        if (!File.Exists(file))
        {
            return treatNullFileAsInvalid;
        }

        return new FileInfo(file).Length == 0;
    }

    private static async ValueTask EnsureThemedMonochromeFileExistsAsync(ImageCacheFile imageCacheFile, ElementTheme theme)
    {
        if (theme is ElementTheme.Default)
        {
            return;
        }

        string themedFilePath = imageCacheFile.GetThemedFile(theme);
        if (!IsFileInvalid(themedFilePath)
            && File.GetLastWriteTimeUtc(themedFilePath) >= File.GetLastWriteTimeUtc(imageCacheFile.DefaultFilePath))
        {
            return;
        }

        try
        {
            // 先写临时文件再原子替换，避免截断旧文件的瞬间被 XAML 解码出「找不到组件」
            string tempFilePath = themedFilePath + ".tmp";
            using (FileStream sourceStream = File.OpenRead(imageCacheFile.DefaultFilePath))
            {
                using (FileStream themeStream = File.Create(tempFilePath))
                {
                    await MonoChromeImageConverter.ConvertAndCopyToAsync(theme, sourceStream, themeStream).ConfigureAwait(false);
                }
            }

            File.Move(tempFilePath, themedFilePath, true);
        }
        catch (IOException ex)
        {
            throw InternalImageCacheException.Throw("Failed to convert image", ex);
        }
    }
}