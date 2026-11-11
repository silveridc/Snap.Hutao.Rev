// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Response;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Calculate;

internal sealed class FurnitureListWrapper : ListWrapper<Item>
{
    [JsonPropertyName("not_calc_list")]
    public List<Item>? NotCalculateList { get; set; }
}