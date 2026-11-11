// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Globalization;

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaServiceEndpoints : IHomaRootAccess
{
    string Announcement(string locale, IEnumerable<long> excludedIds)
    {
        string exclude = string.Join(',', excludedIds.Select(id => id.ToString(CultureInfo.InvariantCulture)));
        return $"{Root}/announcements?locale={Uri.EscapeDataString(locale)}&exclude={exclude}";
    }

    string AnnouncementUpload()
    {
        return $"{Root}/admin/announcements";
    }

    string GachaLogCompensation(int days)
    {
        return $"{Root}/admin/gacha-logs/compensations?days={days}";
    }

    string GachaLogDesignation(string userName, int days)
    {
        return $"{Root}/admin/gacha-logs/compensations/designation?userName={Uri.EscapeDataString(userName)}&days={days}";
    }

    string CdnCompensation(int days)
    {
        return $"{Root}/admin/cdn/compensations?days={days}";
    }

    string CdnDesignation(string userName, int days)
    {
        return $"{Root}/admin/cdn/compensations/designation?userName={Uri.EscapeDataString(userName)}&days={days}";
    }

    string RedeemCodeGenerate()
    {
        return $"{Root}/admin/redeem-codes/generation";
    }
}
