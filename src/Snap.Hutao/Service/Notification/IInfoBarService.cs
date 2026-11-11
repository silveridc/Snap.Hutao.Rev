// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.Service.Notification;

internal interface IInfoBarService
{
    ObservableCollection<InfoBarOptions> Collection { get; }
}