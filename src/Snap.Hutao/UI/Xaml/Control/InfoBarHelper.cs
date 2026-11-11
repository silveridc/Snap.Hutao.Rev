// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;

namespace Snap.Hutao.Rev.UI.Xaml.Control;

[SuppressMessage("", "SH001")]
[DependencyProperty<bool>("IsTextSelectionEnabled", DefaultValue = false, IsAttached = true, TargetType = typeof(InfoBar), NotNull = true)]
public sealed partial class InfoBarHelper;