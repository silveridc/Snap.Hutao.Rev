// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata.Avatar;
using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.ViewModel.Calendar;

internal sealed class CalendarMetadataContext2
{
    public required CalendarMetadataContext MetadataContext { get; init; }

    public required ILookup<MonthAndDay, Avatar> AvatarBirthdays { get; init; }

    public required ILookup<MaterialId, CalendarItem> MaterialItems { get; init; }
}