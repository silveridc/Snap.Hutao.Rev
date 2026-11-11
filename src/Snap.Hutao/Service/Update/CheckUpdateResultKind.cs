// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Update;

internal enum CheckUpdateResultKind
{
    None = 0,
    VersionApiInvalidResponse = 1,
    VersionApiInvalidSha256 = 2,
    AlreadyUpdated = 3,

    UpdateAvailable = 4,

    /// <summary>当前版本低于服务端配置的最低强制版本，必须更新且不可跳过。</summary>
    UpdateRequired = 5,
}
