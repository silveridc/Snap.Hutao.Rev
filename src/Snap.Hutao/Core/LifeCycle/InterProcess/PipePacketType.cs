// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess;

internal enum PipePacketType : byte
{
    None = 0,
    Request = 1,
    Response = 2,
    SessionTermination = 3,
}