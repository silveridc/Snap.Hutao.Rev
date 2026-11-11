// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class HyperLinkName : IDefaultIdentity<HyperLinkNameId>
{
    public required HyperLinkNameId Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
}