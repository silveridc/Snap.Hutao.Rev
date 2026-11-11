// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database;
using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;

namespace Snap.Hutao.Rev.Service.Game.Account;

[Service(ServiceLifetime.Singleton, typeof(IGameAccountRepository))]
internal sealed partial class GameAccountRepository : IGameAccountRepository
{
    [GeneratedConstructor]
    public partial GameAccountRepository(IServiceProvider serviceProvider);

    public partial IServiceProvider ServiceProvider { get; }

    public ObservableReorderableDbCollection<GameAccount> GetGameAccountCollection()
    {
        return this.Query(query => query.ToObservableReorderableDbCollection(ServiceProvider));
    }

    public void AddGameAccount(GameAccount gameAccount)
    {
        this.Add(gameAccount);
    }

    public void UpdateGameAccount(GameAccount gameAccount)
    {
        this.Update(gameAccount);
    }

    public void RemoveGameAccountById(Guid id)
    {
        this.DeleteByInnerId(id);
    }
}