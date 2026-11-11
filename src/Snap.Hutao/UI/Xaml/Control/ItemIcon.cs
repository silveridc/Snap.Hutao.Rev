// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.UI.Xaml.Control;

[DependencyProperty<QualityType>("Quality", DefaultValue = QualityType.QUALITY_NONE, NotNull = true)]
[DependencyProperty<Uri>("Icon")]
[DependencyProperty<double>("IconOpacity", DefaultValue = 1.0, NotNull = true)]
[DependencyProperty<Uri>("Badge")]
internal sealed partial class ItemIcon : Microsoft.UI.Xaml.Controls.Control;