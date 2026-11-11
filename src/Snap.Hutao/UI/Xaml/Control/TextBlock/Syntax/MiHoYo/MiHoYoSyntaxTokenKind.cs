// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.UI.Xaml.Control.TextBlock.Syntax.MiHoYo;

internal enum MiHoYoSyntaxTokenKind
{
    EndOfFile,
    Text,
    LinkOpen,
    LinkClose,
    ColorOpen,
    ColorClose,
    ItalicOpen,
    ItalicClose,
    SpritePreset,
    Parameter,
}