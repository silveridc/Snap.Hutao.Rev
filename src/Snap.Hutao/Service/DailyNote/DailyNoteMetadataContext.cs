// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.DailyNote;

internal class DailyNoteMetadataContext : IMetadataContext,
    IMetadataArrayChapterSource
{
    public ImmutableArray<Chapter> Chapters { get; set; } = default!;
}
