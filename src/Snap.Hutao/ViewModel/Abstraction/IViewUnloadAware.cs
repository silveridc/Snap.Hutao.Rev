// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Property;

namespace Snap.Hutao.Rev.ViewModel.Abstraction;

internal interface IViewUnloadAware
{
    IProperty<bool> IsViewUnloaded { get; }
}