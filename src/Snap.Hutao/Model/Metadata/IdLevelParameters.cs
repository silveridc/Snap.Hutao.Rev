// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class IdLevelParameters<TId, TLevel, TParam> : LevelParameters<TLevel, TParam>
{
    public IdLevelParameters(TId id, TLevel level, ImmutableArray<TParam> parameters)
        : base(level, parameters)
    {
        Id = id;
    }

    public TId Id { get; }
}