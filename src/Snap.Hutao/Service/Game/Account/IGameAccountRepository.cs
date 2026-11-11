// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database;
using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;

namespace Snap.Hutao.Rev.Service.Game.Account;

internal interface IGameAccountRepository : IRepository<GameAccount>
{
    void AddGameAccount(GameAccount gameAccount);

    void RemoveGameAccountById(Guid id);

    ObservableReorderableDbCollection<GameAccount> GetGameAccountCollection();

    void UpdateGameAccount(GameAccount gameAccount);
}