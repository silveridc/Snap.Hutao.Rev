// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.HardChallenge;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Hutao.Rev.Model.Entity;

[Table("hard_challenges")]
internal sealed class HardChallengeEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    public uint ScheduleId { get; set; }

    public string Uid { get; set; } = default!;

    public HardChallengeData HardChallengeData { get; set; } = default!;

    public static HardChallengeEntry Create(string uid, HardChallengeData hardChallengeData)
    {
        return new()
        {
            Uid = uid,
            ScheduleId = hardChallengeData.Schedule.ScheduleId,
            HardChallengeData = hardChallengeData,
        };
    }
}