// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

internal interface IOverseaSupportFactory<out TClient>
{
    TClient Create(bool isOversea);
}