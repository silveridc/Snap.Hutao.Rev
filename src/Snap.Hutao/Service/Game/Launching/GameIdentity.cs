// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.ViewModel.User;

namespace Snap.Hutao.Rev.Service.Game.Launching;

internal sealed class GameIdentity
{
    public UserAndUid? UserAndUid { get; init; }

    public GameAccount? GameAccount { get; init; }

    public static GameIdentity Create(UserAndUid? userAndUid, GameAccount? gameAccount)
    {
        return new()
        {
            UserAndUid = userAndUid,
            GameAccount = gameAccount,
        };
    }

    public static GameIdentity Create()
    {
        return new();
    }
}