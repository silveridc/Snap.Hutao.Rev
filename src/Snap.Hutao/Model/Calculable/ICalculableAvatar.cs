// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Calculable;

internal interface ICalculableAvatar : ICalculablePromoteLevel
{
    AvatarId AvatarId { get; }

    ImmutableArray<ICalculableSkill> Skills { get; }
}