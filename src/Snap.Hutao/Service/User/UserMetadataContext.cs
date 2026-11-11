// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata.Avatar;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.User;

internal class UserMetadataContext : IUserMetadataContext
{
    public ImmutableArray<ProfilePicture> ProfilePictures { get; set; } = default!;
}
