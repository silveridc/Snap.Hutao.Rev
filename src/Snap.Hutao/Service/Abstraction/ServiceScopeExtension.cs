// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Hutao.Rev.Model.Entity.Database;

namespace Snap.Hutao.Rev.Service.Abstraction;

internal static class ServiceScopeExtension
{
    extension(IServiceScope scope)
    {
        public TService GetRequiredService<TService>()
            where TService : class
        {
            return scope.ServiceProvider.GetRequiredService<TService>();
        }

        public TDbContext GetDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return scope.GetRequiredService<TDbContext>();
        }

        public AppDbContext GetAppDbContext()
        {
            return scope.GetDbContext<AppDbContext>();
        }
    }
}