// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess.Yae;

internal enum YaeCommandKind : byte
{
    None = 0,
    ResponseAchievement = 1,
    ResponsePlayerStore = 2,
    ResponsePlayerProp = 3,
    RequestCmdId = 252,
    RequestRva = 253,
    RequestResumeThread = 254,
    SessionEnd = 255,
}