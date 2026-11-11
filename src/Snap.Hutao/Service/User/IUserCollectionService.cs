// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database;
using BindingUser = Snap.Hutao.Rev.ViewModel.User.User;
using EntityUser = Snap.Hutao.Rev.Model.Entity.User;

namespace Snap.Hutao.Rev.Service.User;

internal interface IUserCollectionService
{
    ValueTask<AdvancedDbCollectionView<BindingUser, EntityUser>> GetUsersAsync();

    ValueTask RemoveUserAsync(BindingUser user);

    ValueTask<ValueResult<UserOptionResultKind, string?>> TryCreateAndAddUserFromInputCookieAsync(InputCookie inputCookie);
}