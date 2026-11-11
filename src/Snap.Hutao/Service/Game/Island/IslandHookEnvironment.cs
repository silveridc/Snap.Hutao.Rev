// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.IO.MemoryMappedFiles;

namespace Snap.Hutao.Rev.Service.Game.Island;

// Must match HookEnvironment in Snap.Hutao.Remastered.UnlockerIsland (BOOL/DWORD/FLOAT are all 4 bytes).
// Only the offsets between IsOversea and FastSkipTalk belong to the launcher, State, LastError,
// Uid and the trailing Offsets table are written by the injected module.
internal static class IslandHookEnvironment
{
    public const int Size = 280;
    public const int StateOffset = 4;
    public const int LastErrorOffset = 8;
    public const int IsOverseaOffset = 16;
    public const int ProvideOffsetsOffset = 20;
    public const int EnableSetFovOffset = 24;
    public const int FieldOfViewOffset = 28;
    public const int DisablePlayerPerspectiveOffset = 32;
    public const int DisableFogOffset = 36;
    public const int RemoveTeamProgressOffset = 48;
    public const int HideQuestBannerOffset = 52;
    public const int DisableCameraMoveOffset = 56;
    public const int DisableDamageTextOffset = 60;
    public const int TouchModeOffset = 64;
    public const int RedirectCombineOffset = 68;
    public const int ResinItem000106Offset = 72;
    public const int ResinItem000201Offset = 76;
    public const int ResinItem107009Offset = 80;
    public const int ResinItem107012Offset = 84;
    public const int ResinItem220007Offset = 88;
    public const int DisplayPaimonOffset = 92;
    public const int HidePlayerInfoOffset = 100;
    public const int HideGrassOffset = 104;
    public const int GamepadHotSwitchOffset = 108;
    public const int InLevelClockPageSpeedUpOffset = 112;
    public const int CombineHotkeyOffset = 116;
    public const int WeakMapCheckOffset = 120;
    public const int DisablePlayerDiveMosaicOffset = 124;
    public const int FastSkipTalkOffset = 128;

    public static void WriteFlags(MemoryMappedViewAccessor accessor, LaunchOptions options, int combineHotkey)
    {
        accessor.Write(EnableSetFovOffset, ToBool(options.IsSetFieldOfViewEnabled.Value));
        accessor.Write(FieldOfViewOffset, options.TargetFov.Value);
        accessor.Write(DisablePlayerPerspectiveOffset, ToBool(options.FixLowFovScene.Value));
        accessor.Write(DisableFogOffset, ToBool(options.DisableFog.Value));
        accessor.Write(RemoveTeamProgressOffset, ToBool(options.RemoveOpenTeamProgress.Value));
        accessor.Write(HideQuestBannerOffset, ToBool(options.HideQuestBanner.Value));
        accessor.Write(DisableCameraMoveOffset, ToBool(options.DisableEventCameraMove.Value));
        accessor.Write(DisableDamageTextOffset, ToBool(options.DisableShowDamageText.Value));
        accessor.Write(TouchModeOffset, ToBool(options.UsingTouchScreen.Value));
        accessor.Write(RedirectCombineOffset, ToBool(options.RedirectCombineEntry.Value));
        accessor.Write(ResinItem000106Offset, ToBool(options.ResinListItemId000106Allowed.Value));
        accessor.Write(ResinItem000201Offset, ToBool(options.ResinListItemId000201Allowed.Value));
        accessor.Write(ResinItem107009Offset, ToBool(options.ResinListItemId107009Allowed.Value));
        accessor.Write(ResinItem107012Offset, ToBool(options.ResinListItemId107012Allowed.Value));
        accessor.Write(ResinItem220007Offset, ToBool(options.ResinListItemId220007Allowed.Value));
        accessor.Write(DisplayPaimonOffset, ToBool(options.DisplayPaimon.Value));
        accessor.Write(HidePlayerInfoOffset, ToBool(options.HidePlayerInfo.Value));
        accessor.Write(HideGrassOffset, ToBool(options.HideGrass.Value));
        accessor.Write(GamepadHotSwitchOffset, ToBool(options.GamepadHotSwitch.Value));
        accessor.Write(InLevelClockPageSpeedUpOffset, ToBool(options.InLevelClockPageSpeedUp.Value));
        accessor.Write(CombineHotkeyOffset, combineHotkey);
        accessor.Write(WeakMapCheckOffset, ToBool(options.RelaxInDungeonOpenMapRestrictions.Value));
        accessor.Write(DisablePlayerDiveMosaicOffset, ToBool(options.DisablePlayerDiveMosaic.Value));
        accessor.Write(FastSkipTalkOffset, ToBool(options.FastSkipTalk.Value));
    }

    public static int ToBool(bool value)
    {
        return value ? 1 : 0;
    }
}
