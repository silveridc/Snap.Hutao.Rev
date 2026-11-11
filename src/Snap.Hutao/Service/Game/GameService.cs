// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Model.Entity.Primitive;
using Snap.Hutao.Rev.Service.Game.Account;
using Snap.Hutao.Rev.Service.Game.Configuration;
using Snap.Hutao.Rev.Service.Game.PathAbstraction;
using Snap.Hutao.Rev.UI.Xaml.Data;

namespace Snap.Hutao.Rev.Service.Game;

[Service(ServiceLifetime.Singleton, typeof(IGameService))]
internal sealed partial class GameService : IGameService
{
    private readonly IGameInRegistryAccountService gameInRegistryAccountService;
    private readonly IGameChannelOptionsService gameChannelOptionsService;
    private readonly IGamePathService gamePathService;

    [GeneratedConstructor]
    public partial GameService(IServiceProvider serviceProvider);

    public async ValueTask<IAdvancedCollectionView<GameAccount>> GetGameAccountCollectionAsync()
    {
        return await gameInRegistryAccountService.GetGameAccountCollectionAsync().ConfigureAwait(false);
    }

    public async ValueTask<ValueResult<bool, string>> GetGamePathAsync()
    {
        return await gamePathService.SilentLocateGamePathAsync().ConfigureAwait(false);
    }

    public ChannelOptions GetChannelOptions()
    {
        return gameChannelOptionsService.GetChannelOptions();
    }

    public async ValueTask<GameAccount?> DetectGameAccountAsync(SchemeType scheme, Func<string, Task<ValueResult<bool, string?>>> providerNameCallback)
    {
        return await gameInRegistryAccountService.DetectCurrentGameAccountAsync(scheme, providerNameCallback).ConfigureAwait(false);
    }

    public GameAccount? DetectCurrentGameAccount(SchemeType scheme)
    {
        return gameInRegistryAccountService.DetectCurrentGameAccount(scheme);
    }

    public async ValueTask ModifyGameAccountAsync(GameAccount gameAccount, Func<string, Task<ValueResult<bool, string?>>> providerNameCallback)
    {
        await gameInRegistryAccountService.ModifyGameAccountAsync(gameAccount, providerNameCallback).ConfigureAwait(false);
    }

    public async ValueTask RemoveGameAccountAsync(GameAccount gameAccount)
    {
        await gameInRegistryAccountService.RemoveGameAccountAsync(gameAccount).ConfigureAwait(false);
    }
}