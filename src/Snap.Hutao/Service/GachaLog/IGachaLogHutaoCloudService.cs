// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.ViewModel.GachaLog;
using Snap.Hutao.Rev.Web.Hutao.GachaLog;
using Snap.Hutao.Rev.Web.Hutao.Response;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.GachaLog;

internal interface IGachaLogHutaoCloudService
{
    ValueTask<ValueResult<bool, string>> DeleteGachaItemsAsync(string uid, CancellationToken token = default);

    ValueTask<ValueResult<bool, HutaoStatistics>> GetCurrentEventStatisticsAsync(CancellationToken token = default);

    ValueTask<HutaoResponse<ImmutableArray<GachaEntry>>> GetGachaEntriesAsync(CancellationToken token = default);

    ValueTask<ValueResult<bool, Guid>> RetrieveGachaArchiveIdAsync(string uid, CancellationToken token = default);

    ValueTask<ValueResult<bool, string>> UploadGachaItemsAsync(GachaArchive gachaArchive, CancellationToken token = default);
}