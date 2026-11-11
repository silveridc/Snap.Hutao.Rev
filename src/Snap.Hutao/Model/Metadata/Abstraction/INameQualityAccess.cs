// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Intrinsic;

namespace Snap.Hutao.Rev.Model.Metadata.Abstraction;

internal interface INameQualityAccess : INameAccess
{
    QualityType Quality { get; }
}