// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.HardChallenge;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.HardChallenge;

internal sealed class ChallengeView
{
    private ChallengeView(HardChallengeChallenge challenge, HardChallengeMetadataContext context)
    {
        Name = challenge.Name;
        FormattedSeconds = SH.FormatViewModelHardChallengeSeconds(challenge.Seconds);
        Avatars = [.. challenge.Team.Where(a => context.IdAvatarMap.ContainsKey(a.AvatarId)).Select(a => AvatarView.Create(a, context))];
        BestAvatars = [.. challenge.BestAvatars.Where(a => context.IdAvatarMap.ContainsKey(a.AvatarId)).Select(a => AvatarDamage.Create(a, context))];
        Monster = MonsterView.Create(challenge.Monster, context);
    }

    public string Name { get; }

    public string FormattedSeconds { get; }

    public ImmutableArray<AvatarView> Avatars { get; }

    public ImmutableArray<AvatarDamage> BestAvatars { get; }

    public MonsterView Monster { get; }

    public static ChallengeView Create(HardChallengeChallenge challenge, HardChallengeMetadataContext context)
    {
        return new(challenge, context);
    }
}