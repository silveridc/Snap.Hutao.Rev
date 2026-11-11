// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Abstraction;

internal interface IPinnable<TData>
{
    ref TData GetPinnableReference();
}