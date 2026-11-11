// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaSpiralAbyssEndpoints : IHomaRootAccess
{
    string RecordCheck(string uid)
    {
        return $"{Root}/records/abyss/{Uri.EscapeDataString(uid)}/status";
    }

    string RecordRank(string uid)
    {
        return $"{Root}/records/abyss/{Uri.EscapeDataString(uid)}/rank";
    }

    string RecordUpload()
    {
        return $"{Root}/records/abyss";
    }

    string StatisticsOverview(bool last = false)
    {
        return $"{Root}/statistics/overview?Last={last}";
    }

    string StatisticsAvatarAttendanceRate(bool last = false)
    {
        return $"{Root}/statistics/avatars/attendance-rate?Last={last}";
    }

    string StatisticsAvatarUtilizationRate(bool last = false)
    {
        return $"{Root}/statistics/avatars/utilization-rate?Last={last}";
    }

    string StatisticsAvatarAvatarCollocation(bool last = false)
    {
        return $"{Root}/statistics/avatars/collocations?Last={last}";
    }

    string StatisticsAvatarHoldingRate(bool last = false)
    {
        return $"{Root}/statistics/avatars/holding-rate?Last={last}";
    }

    string StatisticsWeaponWeaponCollocation(bool last = false)
    {
        return $"{Root}/statistics/weapons/collocations?Last={last}";
    }

    string StatisticsTeamCombination(bool last = false)
    {
        return $"{Root}/statistics/teams/combinations?Last={last}";
    }
}
