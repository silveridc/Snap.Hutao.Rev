// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Response;

namespace Snap.Hutao.Rev.Web.Hoyolab.Hk4e.Common.Announcement;

internal sealed class AnnouncementListWrapper : ListWrapper<Announcement>
{
    [JsonPropertyName("type_id")]
    public int TypeId { get; set; }

    [JsonPropertyName("type_label")]
    public string TypeLabel { get; set; } = default!;
}
