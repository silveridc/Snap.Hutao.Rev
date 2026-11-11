// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

internal sealed class UserInfoWrapper
{
    [JsonPropertyName("user_info")]
    public UserInformation UserInfo { get; set; } = default!;
}