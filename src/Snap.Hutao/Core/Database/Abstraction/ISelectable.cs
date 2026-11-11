// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity.Abstraction;

namespace Snap.Hutao.Rev.Core.Database.Abstraction;

internal interface ISelectable : IAppDbEntity
{
    bool IsSelected { get; set; }
}