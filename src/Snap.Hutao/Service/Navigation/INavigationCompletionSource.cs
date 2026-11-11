// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Navigation;

internal interface INavigationCompletionSource
{
    [SuppressMessage("", "SH003")]
    Task WaitForCompletionAsync();
}