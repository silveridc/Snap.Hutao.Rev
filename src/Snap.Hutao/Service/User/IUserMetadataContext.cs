// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;

namespace Snap.Hutao.Rev.Service.User;

internal interface IUserMetadataContext : IMetadataContext,
    IMetadataArrayProfilePictureSource;
