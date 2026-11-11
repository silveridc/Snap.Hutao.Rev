// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.ViewModel.Wiki;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata;

internal static class TypeValueCollectionExtension
{
    extension(TypeValueCollection<FightProperty, GrowCurveType> collection)
    {
        public ImmutableArray<PropertyCurveValue> ToPropertyCurveValues(BaseValue baseValue)
        {
            return collection.Array.SelectAsArray(static (info, baseValue) => new PropertyCurveValue(info.Type, info.Value, baseValue), baseValue);
        }
    }
}