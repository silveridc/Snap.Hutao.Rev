// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.ViewModel.AvatarProperty;

internal class ReliquaryComposedSubProperty : ReliquarySubProperty
{
    public ReliquaryComposedSubProperty(FightProperty type, string value, uint enhancedCount)
        : base(type, value)
    {
        Type = type;
        EnhancedCount = enhancedCount;
    }

    public uint EnhancedCount { get; set; }

    internal FightProperty Type { get; }
}