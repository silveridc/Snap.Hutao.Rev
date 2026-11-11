// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Collection.Generic;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.ViewModel.Cultivation;

namespace Snap.Hutao.Rev.Service.Cultivation;

internal sealed class StatisticsCultivateItemComparer : DelegatingPropertyComparer<StatisticsCultivateItem, MaterialId>
{
    private static readonly LazySlim<StatisticsCultivateItemComparer> LazyShared = new(() => new());

    private StatisticsCultivateItemComparer()
        : base(static i => i.Inner.Id, MaterialIdComparer.Shared)
    {
    }

    public static StatisticsCultivateItemComparer Shared { get => LazyShared.Value; }
}