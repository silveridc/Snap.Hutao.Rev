// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.Service.Abstraction;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Inventory;

internal interface IInventoryRepository : IRepository<InventoryItem>
{
    void AddInventoryItemRangeByProjectId(IEnumerable<InventoryItem> items);

    void RemoveInventoryItemRangeByProjectId(Guid projectId);

    void UpdateInventoryItem(InventoryItem item);

    ImmutableArray<InventoryItem> GetInventoryItemImmutableArrayByProjectId(Guid projectId);

    ImmutableDictionary<uint, InventoryItem> GetInventoryItemImmutableDictionaryByProjectId(Guid projectId);
}