// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Entity.Abstraction;

internal interface IAppDbEntityHasArchive : IAppDbEntity
{
    Guid ArchiveId { get; }
}