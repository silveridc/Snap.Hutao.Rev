// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata.Avatar;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;
using System.Collections.Immutable;
using System.Globalization;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal sealed partial class Team : List<AvatarView>
{
    public Team(ItemRate<string, int> team, ImmutableDictionary<AvatarId, Avatar> idAvatarMap, int rank)
        : base(4)
    {
        ReadOnlySpan<char> itemSpan = team.Item.AsSpan();
        foreach (Range range in itemSpan.Split(','))
        {
            uint id = uint.Parse(itemSpan[range], CultureInfo.InvariantCulture);
            Add(new(idAvatarMap[id], 0));
        }

        AddRange(new AvatarView[4 - Count]);

        UpCount = SH.FormatModelBindingHutaoTeamUpCount(team.Rate);
        Rank = rank;
    }

    public string UpCount { get; }

    public int Rank { get; set; }
}