// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Game.PathAbstraction;

internal interface IGamePathService
{
    ValueTask<ValueResult<bool, string>> SilentLocateGamePathAsync();

    ValueTask SilentLocateAllGamePathAsync();
}