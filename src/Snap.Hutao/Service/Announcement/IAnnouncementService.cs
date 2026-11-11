// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab;
using Snap.Hutao.Rev.Web.Hoyolab.Hk4e.Common.Announcement;

namespace Snap.Hutao.Rev.Service.Announcement;

internal interface IAnnouncementService
{
    ValueTask<AnnouncementWrapper?> GetAnnouncementWrapperAsync(string languageCode, Region region, CancellationToken token = default);
}