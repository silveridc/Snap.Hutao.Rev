// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.Service.Hutao;

internal interface IHutaoAsAService
{
    ValueTask<ObservableCollection<Web.Hutao.HutaoAsAService.Announcement>> GetHutaoAnnouncementCollectionAsync(CancellationToken token = default);
}