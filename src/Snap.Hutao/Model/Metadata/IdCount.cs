// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Model.Metadata;

internal class IdCount
{
    public required MaterialId Id { get; init; }

    public required uint Count { get; init; }
}