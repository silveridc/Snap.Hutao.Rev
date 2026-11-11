// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Collection.Generic;
using Snap.Hutao.Rev.Model.Intrinsic;
using Snap.Hutao.Rev.ViewModel.AvatarProperty;

namespace Snap.Hutao.Rev.Service.AvatarInfo.Factory;

internal sealed class InGameAvatarPropertyComparer : DelegatingPropertyComparer<AvatarProperty, FightProperty>
{
    private static readonly LazySlim<InGameAvatarPropertyComparer> LazyShared = new(() => new());

    private InGameAvatarPropertyComparer()
        : base(static a => a.FightProperty, InGameFightPropertyComparer.Shared)
    {
    }

    public static InGameAvatarPropertyComparer Shared { get => LazyShared.Value; }
}