// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Model.Entity;

namespace Snap.Hutao.Rev.UI.Xaml.View.Dialog;

internal sealed partial class DailyNoteNotificationDialog : ContentDialog
{
    public DailyNoteNotificationDialog(IServiceProvider serviceProvider, DailyNoteEntry entry)
    {
        InitializeComponent();

        DataContext = entry;
    }
}
