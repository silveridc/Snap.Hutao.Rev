// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Game.Package;
using Snap.Hutao.Rev.Service.Game.Scheme;

namespace Snap.Hutao.Rev.ViewModel.Game;

internal interface IViewModelSupportLaunchExecution
{
    LaunchScheme? TargetScheme { get; }

    LaunchScheme? CurrentScheme { get; }

    GameAccount? GameAccount { get; }

    ValueTask<BlockDeferral<PackageConvertStatus>> CreateConvertBlockDeferralAsync();
}