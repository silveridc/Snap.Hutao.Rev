// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal interface IDefaultIdentity<out TIdentity>
{
    TIdentity Id { get; }
}