// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Metadata.Avatar;

internal sealed class ExtraLevelInfo
{
    public required ExtraLevelIndexKind Index { get; init; }

    public required uint Level { get; init; }
}