// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.UI.Xaml.Control.TextBlock.Syntax.MiHoYo;

internal sealed class MiHoYoSyntaxTextElement : MiHoYoSyntaxElement
{
    public MiHoYoSyntaxTextElement(TextPosition position, ImmutableArray<MiHoYoSyntaxElement> children)
        : base(position, children)
    {
    }
}