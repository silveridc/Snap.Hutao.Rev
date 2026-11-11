// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao;

internal sealed class UserInfo
{
    public required string NormalizedUserName { get; init; }

    public required string UserName { get; init; }

    public required bool IsLicensedDeveloper { get; init; }

    public required bool IsMaintainer { get; init; }

    /// <summary>是否允许写维护接口，单点登录启用后由「账号绑定了后台账号」决定，老服务端的响应里没有这个字段。</summary>
    public bool CanWrite { get; init; }

    public required DateTimeOffset GachaLogExpireAt { get; init; }

    public required DateTimeOffset CdnExpireAt { get; init; }
}