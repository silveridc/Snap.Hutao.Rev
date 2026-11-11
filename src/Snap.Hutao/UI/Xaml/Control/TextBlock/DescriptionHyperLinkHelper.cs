// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Documents;
using Snap.Hutao.Rev.UI.Xaml.Control.TextBlock.Syntax.MiHoYo;

namespace Snap.Hutao.Rev.UI.Xaml.Control.TextBlock;

[DependencyProperty<Tuple<MiHoYoSyntaxLinkKind, uint>>("LinkData", IsAttached = true, TargetType = typeof(Hyperlink))]
internal partial class DescriptionHyperLinkHelper
{
}