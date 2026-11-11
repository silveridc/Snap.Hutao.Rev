// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Web.Hoyolab;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Snap.Hutao.Rev.Service;

internal static class KnownServerRegionTimeZones
{
    public static ImmutableArray<NameValue<TimeSpan>> Value
    {
        get
        {
            Debug.Assert(XamlApplicationLifetime.CultureInfoInitialized);
            return !field.IsDefault ? field : field =
            [
                new(SH.ServiceAppOptionsCalendarServerTimeZoneCommon, ServerRegionTimeZone.CommonOffset),
                new(SH.ServiceAppOptionsCalendarServerTimeZoneAmerica, ServerRegionTimeZone.AmericaServerOffset),
                new(SH.ServiceAppOptionsCalendarServerTimeZoneEurope, ServerRegionTimeZone.EuropeServerOffset),
            ];
        }
    }
}