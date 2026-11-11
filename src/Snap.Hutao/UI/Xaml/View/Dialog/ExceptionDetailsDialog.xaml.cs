// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;

namespace Snap.Hutao.Rev.UI.Xaml.View.Dialog;

[DependencyProperty<string>("Details", NotNull = true)]
internal sealed partial class ExceptionDetailsDialog : ContentDialog
{
    public ExceptionDetailsDialog(IServiceProvider serviceProvider, string details)
    {
        InitializeComponent();
        Details = details;
    }
}
