// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Navigation;

namespace Snap.Hutao.Rev.ViewModel.Game;

internal sealed class LaunchGameExtraData : NavigationExtraData<string>
{
    private LaunchGameExtraData(string uid)
        : base(uid)
    {
    }

    public static INavigationCompletionSource CreateForUid(string? uid)
    {
        return uid is null ? Default : new LaunchGameExtraData(uid);
    }
}