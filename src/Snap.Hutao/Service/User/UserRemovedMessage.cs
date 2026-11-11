// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using BindingUser = Snap.Hutao.Rev.ViewModel.User.User;

namespace Snap.Hutao.Rev.Service.User;

internal sealed class UserRemovedMessage
{
    public UserRemovedMessage(BindingUser removedUser)
    {
        RemovedUser = removedUser;
    }

    public BindingUser RemovedUser { get; }
}