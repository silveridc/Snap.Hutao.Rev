// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;
using Snap.Hutao.Rev.ViewModel.AvatarProperty;

namespace Snap.Hutao.Rev.Service.AvatarInfo.Factory.Builder;

internal interface INameIconDescriptionBuilder<out T> : IBuilder
    where T : NameIconDescription
{
    T View { get; }
}