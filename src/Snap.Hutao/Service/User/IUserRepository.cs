// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Abstraction;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.User;

internal interface IUserRepository : IRepository<Model.Entity.User>
{
    void DeleteUserById(Guid id);

    ImmutableArray<Model.Entity.User> GetUserImmutableArray();

    void UpdateUser(Model.Entity.User user);

    void ClearUserSelection();
}