// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.Model.Metadata;

namespace Snap.Hutao.Rev.ViewModel.Wiki;

internal sealed class PropertyCurveValue
{
    public PropertyCurveValue(FightProperty property, GrowCurveType type, float value)
    {
        Property = property;
        Type = type;
        Value = value;
    }

    public PropertyCurveValue(FightProperty property, GrowCurveType type, BaseValue baseValue)
        : this(property, type, baseValue.GetValue(property))
    {
    }

    public FightProperty Property { get; }

    public GrowCurveType Type { get; }

    public float Value { get; }
}