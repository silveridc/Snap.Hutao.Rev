// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Cultivation;
using Snap.Hutao.Rev.ViewModel.Cultivation;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Inventory;

internal interface IInventoryService
{
    ImmutableArray<InventoryItemView> GetInventoryItemViews(ICultivationMetadataContext context, CultivateProject cultivateProject, ICommand saveCommand);

    void RemoveInventoryItems(CultivateProject cultivateProject);

    void SaveInventoryItem(InventoryItemView item);

    ValueTask RefreshInventoryAsync(RefreshOptions refreshOptions);
}