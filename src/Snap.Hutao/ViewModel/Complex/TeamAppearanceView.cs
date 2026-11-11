// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Metadata.Avatar;
using Snap.Hutao.Rev.Model.Primitive;
using Snap.Hutao.Rev.Web.Hutao.SpiralAbyss;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Complex;

internal sealed class TeamAppearanceView
{
    public TeamAppearanceView(TeamAppearance teamRank, ImmutableDictionary<AvatarId, Avatar> idAvatarMap)
    {
        Floor = SH.FormatModelBindingHutaoComplexRankFloor(teamRank.Floor);
        Up = teamRank.Up.SelectAsArray(static (teamRate, index, idAvatarMap) => new Team(teamRate, idAvatarMap, index + 1), idAvatarMap);
        Down = teamRank.Down.SelectAsArray(static (teamRate, index, idAvatarMap) => new Team(teamRate, idAvatarMap, index + 1), idAvatarMap);
    }

    public string Floor { get; }

    public ImmutableArray<Team> Up { get; }

    public ImmutableArray<Team> Down { get; }
}