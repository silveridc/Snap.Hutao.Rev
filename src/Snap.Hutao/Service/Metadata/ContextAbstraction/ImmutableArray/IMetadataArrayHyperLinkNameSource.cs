// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Metadata.ContextAbstraction.ImmutableArray;

internal interface IMetadataArrayHyperLinkNameSource
{
    ImmutableArray<HyperLinkName> HyperLinkNames { get; set; }
}