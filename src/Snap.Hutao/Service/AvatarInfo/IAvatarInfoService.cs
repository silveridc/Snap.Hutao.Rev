// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.AvatarInfo.Factory;
using Snap.Hutao.Rev.ViewModel.AvatarProperty;
using Snap.Hutao.Rev.ViewModel.User;

namespace Snap.Hutao.Rev.Service.AvatarInfo;

internal interface IAvatarInfoService
{
    ValueTask<Summary?> GetSummaryAsync(SummaryFactoryMetadataContext context, UserAndUid userAndUid, RefreshOptionKind refreshOptionKind, CancellationToken token = default);
}