// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Response;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Miyolive;

internal interface IMiyoliveClient
{
    ValueTask<Response<CodeListWrapper>> RefreshCodeAsync(string actId, CancellationToken token = default);
}