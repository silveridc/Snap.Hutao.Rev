// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Design;

namespace Snap.Hutao.Rev.Model.Entity.Database;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public AppDbContext CreateDbContext(string[] args)
    {
#if DEBUG
        // TODO: replace with your own database file path.
        string userdataDbName = @"D:\Hutao\Userdata.db";
        return AppDbContext.Create(default!, $"Data Source={userdataDbName}");
#else
        throw Core.ExceptionService.HutaoException.NotSupported();
#endif
    }
}
