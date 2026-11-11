// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Game.Locator;

internal interface IGameLocator
{
    ValueTask<ValueResult<bool, string>> LocateSingleGamePathAsync();
}