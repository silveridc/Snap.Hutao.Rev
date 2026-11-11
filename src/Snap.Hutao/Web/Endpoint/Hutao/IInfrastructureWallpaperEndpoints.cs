// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureWallpaperEndpoints : IInfrastructureRootAccess
{
    private string HomaRoot => ((IHomaRootAccess)this).Root;

    string WallpaperBing() => $"{HomaRoot}/wallpapers/bing";

    string WallpaperGenshinLauncher() => $"{HomaRoot}/wallpapers/hoyoplay";

    string WallpaperToday() => $"{HomaRoot}/wallpapers/today";
}
