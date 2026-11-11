// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Hk4e.Event.GachaInfo;

internal static class GachaConfigTypeExtension
{
    extension(GachaType configType)
    {
        public GachaType ToQueryType()
        {
            return configType switch
            {
                GachaType.SpecialActivityAvatar => GachaType.ActivityAvatar,
                _ => configType,
            };
        }
    }
}