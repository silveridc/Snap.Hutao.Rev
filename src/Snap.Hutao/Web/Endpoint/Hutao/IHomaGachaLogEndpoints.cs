// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hutao.GachaLog;

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaGachaLogEndpoints : IHomaRootAccess
{
    string GachaLogEndIds(string uid)
    {
        return $"{Root}/gacha-logs/end-ids?uid={Uri.EscapeDataString(uid)}";
    }

    string GachaLogRetrieve()
    {
        return $"{Root}/gacha-logs/retrievals";
    }

    string GachaLogUpload()
    {
        return $"{Root}/gacha-logs";
    }

    string GachaLogEntries()
    {
        return $"{Root}/gacha-logs/entries";
    }

    string GachaLogDelete(string uid)
    {
        return $"{Root}/gacha-logs/{Uri.EscapeDataString(uid)}";
    }

    string GachaLogStatisticsCurrentEvents()
    {
        return $"{Root}/gacha-logs/statistics/current-event";
    }

    string GachaLogStatisticsDistribution(GachaDistributionType distributionType)
    {
        string slug = distributionType switch
        {
            GachaDistributionType.AvatarEvent => "avatar-event",
            GachaDistributionType.WeaponEvent => "weapon-event",
            GachaDistributionType.Chronicled => "chronicled",
            GachaDistributionType.Standard => "standard",
            _ => throw new ArgumentOutOfRangeException(nameof(distributionType)),
        };

        return $"{Root}/gacha-logs/statistics/distributions/{slug}";
    }
}
