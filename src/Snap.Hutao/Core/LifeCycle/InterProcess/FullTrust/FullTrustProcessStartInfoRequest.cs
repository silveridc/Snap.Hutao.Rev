// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.System.Threading;

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess.FullTrust;

internal sealed class FullTrustProcessStartInfoRequest
{
    public required string ApplicationName { get; set; }

    public required string CommandLine { get; set; }

    public required PROCESS_CREATION_FLAGS CreationFlags { get; set; }

    public required string CurrentDirectory { get; set; }
}