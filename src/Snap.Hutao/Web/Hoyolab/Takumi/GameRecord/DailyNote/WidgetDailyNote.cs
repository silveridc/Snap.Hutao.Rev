// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.DailyNote;

internal sealed class WidgetDailyNote : DailyNoteCommon
{
    /// <summary>
    /// 是否签到
    /// </summary>
    [JsonPropertyName("has_signed")]
    public bool HasSigned { get; set; }

    /// <summary>
    /// 签到页面链接
    /// </summary>
    [JsonPropertyName("sign_url")]
    public Uri SignUrl { get; set; } = default!;
}