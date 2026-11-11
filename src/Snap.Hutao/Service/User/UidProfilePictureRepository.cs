// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;

namespace Snap.Hutao.Rev.Service.User;

[Service(ServiceLifetime.Singleton, typeof(IUidProfilePictureRepository))]
internal sealed partial class UidProfilePictureRepository : IUidProfilePictureRepository
{
    [GeneratedConstructor]
    public partial UidProfilePictureRepository(IServiceProvider serviceProvider);

    public partial IServiceProvider ServiceProvider { get; }

    public UidProfilePicture? SingleUidProfilePictureOrDefaultByUid(string uid)
    {
        try
        {
            return this.Query(query => query.SingleOrDefault(n => n.Uid == uid));
        }
        catch
        {
            this.Delete(n => n.Uid == uid);
            return default;
        }
    }

    public void UpdateUidProfilePicture(UidProfilePicture profilePicture)
    {
        this.Update(profilePicture);
    }

    public void DeleteUidProfilePictureByUid(string uid)
    {
        this.Delete(profilePicture => profilePicture.Uid == uid);
    }
}