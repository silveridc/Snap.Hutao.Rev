// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal interface INameIcon<out TIcon>
{
    string Name { get; }

    TIcon Icon { get; }
}