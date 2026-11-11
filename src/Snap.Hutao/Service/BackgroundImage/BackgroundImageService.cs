// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using JetBrains.Annotations;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.Caching;
using Snap.Hutao.Rev.Core.Graphics.Imaging;
using Snap.Hutao.Rev.Core.IO;
using Snap.Hutao.Rev.UI;
using Snap.Hutao.Rev.Web.Hutao.Wallpaper;
using Snap.Hutao.Rev.Web.Response;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Graphics.Imaging;

namespace Snap.Hutao.Rev.Service.BackgroundImage;

[Service(ServiceLifetime.Singleton, typeof(IBackgroundImageService))]
internal sealed partial class BackgroundImageService : IBackgroundImageService
{
    private static readonly FrozenSet<string> AllowedFormats = [".bmp", ".gif", ".ico", ".jpg", ".jpeg", ".png", ".tiff", ".webp"];

    private readonly BackgroundImageOptions backgroundImageOptions;
    private readonly IServiceProvider serviceProvider;
    private readonly ITaskContext taskContext;
    private readonly AppOptions appOptions;

    // 同一张壁纸的强调色按 (路径, 最后写入时间) 缓存
    private readonly ConcurrentDictionary<string, (long LastWriteTicks, Rgba32 Color)> accentColorCache = [];

    private HashSet<string>? availableBackgroundPathSet;

    [GeneratedConstructor]
    public partial BackgroundImageService(IServiceProvider serviceProvider);

    public async ValueTask<ValueResult</* shouldRefresh */ bool, BackgroundImage?>> GetNextBackgroundImageAsync(BackgroundImage? previous, CancellationToken token = default)
    {
        // backgroundImageOptions.Wallpaper will also be set in this method if web wallpaper type is selected
        HashSet<string> availableBackgroundSet = await SkipOrInitAvailableBackgroundAsync(previous, token).ConfigureAwait(false);

        // The availableBackgroundSet will be empty if BackgroundImageType is None
        if (availableBackgroundSet.Count <= 0)
        {
            return new(previous is not null, default!);
        }

        string path = System.Random.Shared.GetItems([.. availableBackgroundSet], 1)[0];
        availableBackgroundSet.Remove(path);

        if (string.Equals(path, previous?.Path, StringComparison.OrdinalIgnoreCase))
        {
            return new(false, default!);
        }

        if (!File.Exists(path))
        {
            Debugger.Break();
            return new(false, default!);
        }

        FileStream fileStream;
        try
        {
            fileStream = File.OpenRead(path);
        }
        catch (IOException)
        {
            // The process cannot access the file '?' because it is being used by another process.
            return new(false, default!);
        }

        using (fileStream)
        {
            long lastWriteTicks = File.GetLastWriteTimeUtc(path).Ticks;
            Rgba32 accentColor;
            if (accentColorCache.TryGetValue(path, out (long LastWriteTicks, Rgba32 Color) cached) && cached.LastWriteTicks == lastWriteTicks)
            {
                accentColor = cached.Color;
            }
            else
            {
                try
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());

                    // 只需平均色，让解码器先缩到 16x16，避免整图解码和全像素遍历
                    BitmapTransform transform = new()
                    {
                        ScaledWidth = 16,
                        ScaledHeight = 16,
                        InterpolationMode = BitmapInterpolationMode.Fant,
                    };

                    using (SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Straight, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage))
                    {
                        accentColor = softwareBitmap.GetRgba32AccentColor();
                    }

                    accentColorCache[path] = (lastWriteTicks, accentColor);
                }
                catch (COMException comException)
                {
                    if (appOptions.BackgroundImageType.Value is not BackgroundImageType.LocalFolder)
                    {
                        // For web wallpaper, skip invalid file, as users can't control the file
                        return new(false, default!);
                    }

                    // 0xC00D36BE MF_E_INVALID_FILE_FORMAT: Throw to let user know which file is invalid
                    // E_FAIL
                    if (comException.HResult is not unchecked((int)0x80004005))
                    {
                        comException.Data.Add("FilePath", path);
                        comException.Data.Add("HResult", $"0x{comException.HResult:X8}");
                        throw;
                    }

                    return new(false, default!);
                }
            }

            await taskContext.SwitchToMainThreadAsync();

            BackgroundImage background = new()
            {
                Path = path,
                AccentColor = accentColor,
                Luminance = accentColor.Luminance,
            };

            return new(true, background);
        }
    }

    private async ValueTask<HashSet<string>> SkipOrInitAvailableBackgroundAsync(BackgroundImage? previous, CancellationToken token = default)
    {
        switch (appOptions.BackgroundImageType.Value)
        {
            case BackgroundImageType.LocalFolder:
                {
                    if (availableBackgroundPathSet is not { Count: > 0 })
                    {
                        string backgroundFolder = HutaoRuntime.GetDataBackgroundDirectory();

                        availableBackgroundPathSet =
                        [
                            .. Directory
                                .EnumerateFiles(backgroundFolder, "*", SearchOption.AllDirectories)
                                .Where(path => AllowedFormats.Contains(Path.GetExtension(path)))
                        ];

                        // Why > 1: If only one file in the folder, don't change background
                        if (previous is not null && availableBackgroundPathSet.Count > 1)
                        {
                            availableBackgroundPathSet.Remove(previous.Path);
                        }
                    }

                    await taskContext.SwitchToMainThreadAsync();
                    backgroundImageOptions.Wallpaper = default;
                    break;
                }

            case BackgroundImageType.HutaoBing:
                await SetCurrentBackgroundPathSetAsync(static (client, token) => client.GetBingWallpaperAsync(token), token).ConfigureAwait(false);
                break;
            case BackgroundImageType.HutaoDaily:
                await SetCurrentBackgroundPathSetAsync(static (client, token) => client.GetTodayWallpaperAsync(token), token).ConfigureAwait(false);
                break;
            case BackgroundImageType.HutaoOfficialLauncher:
                await SetCurrentBackgroundPathSetAsync(static (client, token) => client.GetLauncherWallpaperAsync(token), token).ConfigureAwait(false);
                break;
            default:
                availableBackgroundPathSet = [];
                break;
        }

        return availableBackgroundPathSet ??= [];
    }

    private async ValueTask SetCurrentBackgroundPathSetAsync([RequireStaticDelegate] Func<HutaoWallpaperClient, CancellationToken, ValueTask<Response<Wallpaper>>> responseFactory, CancellationToken token = default)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            HutaoWallpaperClient wallpaperClient = scope.ServiceProvider.GetRequiredService<HutaoWallpaperClient>();
            Response<Wallpaper> response = await responseFactory(wallpaperClient, token).ConfigureAwait(false);
            if (response is { Data: { } wallpaper })
            {
                if (wallpaper.Url is { } url)
                {
                    try
                    {
                        ValueFile file = await scope.ServiceProvider.GetRequiredService<IImageCache>().GetFileFromCacheAsync(url).ConfigureAwait(false);
                        availableBackgroundPathSet = [file];
                    }
                    catch (InternalImageCacheException)
                    {
                        availableBackgroundPathSet = [];
                    }
                }

                await taskContext.SwitchToMainThreadAsync();
                backgroundImageOptions.Wallpaper = wallpaper;
            }
        }
    }
}