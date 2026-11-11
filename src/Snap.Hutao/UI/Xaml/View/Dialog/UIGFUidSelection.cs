// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.UI.Xaml.View.Dialog;

internal sealed partial class UIGFUidSelection
{
    public UIGFUidSelection(uint uid)
    {
        Uid = uid;
    }

    public uint Uid { get; }

    public static UIGFUidSelection Create(uint uid)
    {
        return new(uid);
    }

    public static ImmutableArray<uint> GetSelectedUidArray(ListViewBase listView)
    {
        return [.. listView.SelectedItems.Cast<UIGFUidSelection>().Select(static data => data.Uid)];
    }
}