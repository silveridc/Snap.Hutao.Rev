// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.AvatarProperty;

namespace Snap.Hutao.Rev.Service.AvatarInfo.Factory;

internal interface ISummaryFactory
{
    ValueTask<Summary> CreateAsync(SummaryFactoryMetadataContext context, IEnumerable<Model.Entity.AvatarInfo> avatarInfos, CancellationToken token);
}