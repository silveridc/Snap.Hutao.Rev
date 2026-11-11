// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Primitive;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.Avatar;

internal sealed class Skill
{
    [JsonPropertyName("skill_id")]
    public SkillId SkillId { get; set; }

    [JsonPropertyName("skill_type")]
    public SkillType SkillType { get; set; }

    [JsonPropertyName("level")]
    public SkillLevel Level { get; set; }

    // Ignored field string desc
    // Ignored field * skill_affix_list
}