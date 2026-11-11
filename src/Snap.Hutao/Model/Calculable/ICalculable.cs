// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Model.Calculable;

internal interface ICalculable : INameIcon<Uri>
{
    QualityType Quality { get; }

    uint LevelCurrent { get; set; }

    uint LevelTarget { get; set; }
}