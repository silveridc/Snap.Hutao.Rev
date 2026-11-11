// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Database;
using Snap.Hutao.Rev.Model.Entity;

namespace Snap.Hutao.Rev.Service.GachaLog;

internal static class GachaArchiveOperation
{
    public static GachaArchive GetOrAdd(IGachaLogRepository repository, string uid, IAdvancedDbCollectionView<GachaArchive> archives)
    {
        GachaArchive? archive = archives.Source.SingleOrDefault(a => a.Uid == uid);

        if (archive is not null)
        {
            return archive;
        }

        GachaArchive created = GachaArchive.Create(uid);
        repository.AddGachaArchive(created);
        CollectionViewAddGachaArchive(archives, created, repository.ServiceProvider.GetRequiredService<ITaskContext>());
        return created;
    }

    private static void CollectionViewAddGachaArchive(IAdvancedDbCollectionView<GachaArchive> archives, GachaArchive archive, ITaskContext taskContext)
    {
        taskContext.InvokeOnMainThread(() => archives.Add(archive));
    }
}