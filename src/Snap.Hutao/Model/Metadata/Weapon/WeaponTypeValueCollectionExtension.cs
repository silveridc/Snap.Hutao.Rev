// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.Wiki;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Model.Metadata.Weapon;

internal static class WeaponTypeValueCollectionExtension
{
    extension(WeaponTypeValueCollection collection)
    {
        public ImmutableArray<PropertyCurveValue> ToPropertyCurveValues()
        {
            return collection.Array.SelectAsArray(static curve => new PropertyCurveValue(curve.Type, curve.Value, curve.InitValue));
        }
    }
}