// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Property;

internal interface IReadOnlyProperty<T>
{
    T Value { get; }
}