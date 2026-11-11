// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Response;

namespace Snap.Hutao.Rev.Web.Hoyolab.Bbs.Home;

internal interface IHomeClient
{
    ValueTask<Response<NewHomeNewInfo>> GetNewHomeInfoAsync(int gid, CancellationToken token = default);
}