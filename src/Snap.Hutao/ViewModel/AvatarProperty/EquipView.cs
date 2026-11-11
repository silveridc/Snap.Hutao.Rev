// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.ViewModel.AvatarProperty;

internal abstract class EquipView : NameIconDescription
{
    public string Level { get; set; } = default!;

    public QualityType Quality { get; set; }

    public NameValue<string>? MainProperty { get; set; }

    internal EquipType EquipType { get; set; }
}
