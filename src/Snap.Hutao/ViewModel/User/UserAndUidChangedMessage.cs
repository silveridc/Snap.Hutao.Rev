// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.ViewModel.User;

internal sealed class UserAndUidChangedMessage
{
    public static readonly UserAndUidChangedMessage Empty = new(null);

    public UserAndUidChangedMessage(User? user)
    {
        User = user;
        if (UserAndUid.TryFromUser(user, out UserAndUid? userAndUid))
        {
            UserAndUid = userAndUid;
        }
    }

    public User? User { get; set; }

    public UserAndUid? UserAndUid { get; }

    public static UserAndUidChangedMessage FromUser(User? user)
    {
        return new(user);
    }
}