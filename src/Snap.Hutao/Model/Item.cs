// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Model;

internal class Item : INameIcon<Uri>
{
    public string Name { get; init; } = default!;

    public Uri Icon { get; init; } = default!;

    public Uri Badge { get; init; } = default!;

    public QualityType Quality { get; init; }

    internal uint Id { get; init; }
}