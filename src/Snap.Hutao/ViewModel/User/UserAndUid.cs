// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab;
using EntityUser = Snap.Hutao.Rev.Model.Entity.User;

namespace Snap.Hutao.Rev.ViewModel.User;

internal sealed class UserAndUid
{
    public UserAndUid(EntityUser user, in PlayerUid role)
    {
        User = user;
        Uid = role;
    }

    public EntityUser User { get; }

    public PlayerUid Uid { get; }

    public bool IsOversea { get => User.IsOversea; }

    public static UserAndUid From(EntityUser user, PlayerUid role)
    {
        return new(user, role);
    }

    public static bool TryFromUser([NotNullWhen(true)] User? user, [NotNullWhen(true)] out UserAndUid? userAndUid)
    {
        if (user is { UserGameRoles.CurrentItem: { } role })
        {
            userAndUid = new(user.Entity, role);
            return true;
        }

        userAndUid = null;
        return false;
    }
}