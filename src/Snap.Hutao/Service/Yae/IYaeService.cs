// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.InterChange.Achievement;
using Snap.Hutao.Rev.Model.InterChange.Inventory;
using Snap.Hutao.Rev.ViewModel.Game;

namespace Snap.Hutao.Rev.Service.Yae;

internal interface IYaeService
{
    ValueTask<UIAF?> GetAchievementAsync(IViewModelSupportLaunchExecution viewModel);

    ValueTask<UIIF?> GetInventoryAsync(IViewModelSupportLaunchExecution viewModel);
}