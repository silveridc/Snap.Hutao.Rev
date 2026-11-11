// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;
using System.Globalization;

namespace Snap.Hutao.Rev.ViewModel.AvatarProperty;

internal class ReliquarySubProperty
{
    public ReliquarySubProperty(FightProperty type, string value)
    {
        Name = type.GetLocalizedDescription(SH.ResourceManager, CultureInfo.CurrentCulture);
        Value = value;
    }

    public string? Name { get; }

    public string Value { get; }
}