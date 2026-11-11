// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.UI.Xaml.Data;

namespace Snap.Hutao.Rev.ViewModel.AvatarProperty;

internal sealed class Summary
{
    public IAdvancedCollectionView<AvatarView> Avatars { get; set; } = default!;

    public static Summary Create(IAdvancedCollectionView<AvatarView> avatars)
    {
        return new()
        {
            Avatars = avatars,
        };
    }
}