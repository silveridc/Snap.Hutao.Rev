// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;
using Snap.Hutao.Rev.UI.Xaml;

namespace Snap.Hutao.Rev.ViewModel.Abstraction;

internal interface IViewModel : IPageScoped, IResurrectable, IViewUnloadAware
{
    CancellationToken CancellationToken { get; set; }

    SemaphoreSlim CriticalSection { get; }

    IDeferContentLoader? DeferContentLoader { get; set; }

    void Uninitialize();
}