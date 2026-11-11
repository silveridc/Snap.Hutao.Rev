// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity.Primitive;
using Snap.Hutao.Rev.Service.Cultivation.Consumption;
using System.Collections.Immutable;
using CalculateItem = Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Calculate.Item;

namespace Snap.Hutao.Rev.Service.Cultivation;

internal sealed class InputConsumption
{
    public required CultivateType Type { get; init; }

    public required uint ItemId { get; init; }

    public required ImmutableArray<CalculateItem> Items { get; init; }

    public required LevelInformation LevelInformation { get; init; }

    public required ConsumptionSaveStrategyKind Strategy { get; init; }
}