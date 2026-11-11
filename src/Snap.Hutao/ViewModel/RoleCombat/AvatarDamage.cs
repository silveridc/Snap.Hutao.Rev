// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata.Avatar;
using System.Globalization;

namespace Snap.Hutao.Rev.ViewModel.RoleCombat;

internal sealed class AvatarDamage : AvatarView
{
    public AvatarDamage(string value, Avatar metaAvatar)
        : base(metaAvatar)
    {
        int.TryParse(value, CultureInfo.InvariantCulture, out int result);
        Value = result;
    }

    public int Value { get; }
}