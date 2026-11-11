// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess.BetterGenshinImpact.Cultivation;

internal sealed class AutomationCultivationEntry
{
    public required uint ItemId { get; set; }

    public required ImmutableArray<AutomationCultivationItem> Items { get; set; }
}