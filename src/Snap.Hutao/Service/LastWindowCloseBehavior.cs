// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service;

[ExtendedEnum]
internal enum LastWindowCloseBehavior
{
    [LocalizationKey(nameof(SH.ServiceCloseButtonBehaviorTypeMinimize))]
    EnsureNotifyIconCreated,

    [LocalizationKey(nameof(SH.ServiceCloseButtonBehaviorTypeExit))]
    ExitApplication,
}