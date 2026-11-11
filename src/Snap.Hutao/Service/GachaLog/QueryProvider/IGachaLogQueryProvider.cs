// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.GachaLog.QueryProvider;

internal interface IGachaLogQueryProvider
{
    ValueTask<ValueResult<bool, GachaLogQuery>> GetQueryAsync();
}