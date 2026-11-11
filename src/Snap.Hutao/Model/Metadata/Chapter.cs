// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class Chapter
{
    public required ChapterId Id { get; init; }

    public required ChapterGroupId GroupId { get; init; }

    public required QuestId BeginQuestId { get; init; }

    public required QuestId EndQuestId { get; init; }

    public required uint NeedPlayerLevel { get; init; }

    public string? Number { get; init; }

    public string? Title { get; init; }

    public required string Icon { get; init; }

    public string? ImageTitle { get; init; }

    public required string SerialNumberIcon { get; init; }

    public required City CityId { get; init; }

    public required QuestType QuestType { get; init; }
}
