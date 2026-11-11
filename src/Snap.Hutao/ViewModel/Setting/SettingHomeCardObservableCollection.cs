// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Snap.Hutao.Rev.ViewModel.Setting;

internal sealed class SettingHomeCardObservableCollection : ObservableCollection<SettingHomeCardViewModel>
{
    public SettingHomeCardObservableCollection(List<SettingHomeCardViewModel> items)
        : base(items)
    {
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        foreach ((int index, SettingHomeCardViewModel item) in Items.Index())
        {
            item.Order = index;
        }
    }
}