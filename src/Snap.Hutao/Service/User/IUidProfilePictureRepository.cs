// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;

namespace Snap.Hutao.Rev.Service.User;

internal interface IUidProfilePictureRepository : IRepository<UidProfilePicture>
{
    UidProfilePicture? SingleUidProfilePictureOrDefaultByUid(string uid);

    void UpdateUidProfilePicture(UidProfilePicture profilePicture);

    void DeleteUidProfilePictureByUid(string uid);
}