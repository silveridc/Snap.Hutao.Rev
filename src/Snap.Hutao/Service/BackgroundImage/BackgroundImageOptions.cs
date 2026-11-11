// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Web.Hutao.Wallpaper;

namespace Snap.Hutao.Rev.Service.BackgroundImage;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class BackgroundImageOptions : ObservableObject
{
    [ObservableProperty]
    public partial Wallpaper? Wallpaper { get; set; }
}