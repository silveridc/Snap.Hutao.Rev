// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Model.Entity.Primitive;
using Snap.Hutao.Rev.Service.Game.Configuration;
using Snap.Hutao.Rev.UI.Xaml.Data;

namespace Snap.Hutao.Rev.Service.Game;

internal interface IGameService
{
    ValueTask<GameAccount?> DetectGameAccountAsync(SchemeType scheme, Func<string, Task<ValueResult<bool, string?>>> providerNameCallback);

    ValueTask<ValueResult<bool, string>> GetGamePathAsync();

    ChannelOptions GetChannelOptions();

    ValueTask ModifyGameAccountAsync(GameAccount gameAccount, Func<string, Task<ValueResult<bool, string?>>> providerNameCallback);

    ValueTask RemoveGameAccountAsync(GameAccount gameAccount);

    GameAccount? DetectCurrentGameAccount(SchemeType scheme);

    ValueTask<IAdvancedCollectionView<GameAccount>> GetGameAccountCollectionAsync();
}