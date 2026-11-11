// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Metadata.Item;

namespace Snap.Hutao.Rev.ViewModel.Cultivation;

internal sealed partial class InventoryItemView : ObservableObject, IEntityAccessWithMetadata<Model.Entity.InventoryItem, Material>
{
    public InventoryItemView(Model.Entity.InventoryItem entity, Material inner, ICommand saveCommand)
    {
        Entity = entity;
        Inner = inner;
        SaveCountCommand = saveCommand;
    }

    public Model.Entity.InventoryItem Entity { get; }

    public Material Inner { get; }

    public ICommand SaveCountCommand { get; }

    public uint Count
    {
        get => Entity.Count; set
        {
            if (SetProperty(Entity.Count, value, Entity, (entity, count) => entity.Count = count))
            {
                SaveCountCommand.Execute(this);
            }
        }
    }
}
