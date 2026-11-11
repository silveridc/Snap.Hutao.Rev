// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess;

internal static class PrivateNamedPipe
{
    public const int PrivateVersion = 1;
    public const int FullTrustVersion = 3;
    public const string PrivateName = "Snap.Hutao.PrivateNamedPipe";
    public const string FullTrustName = "Snap.Hutao.PrivateFullTrustNamedPipe.FpsV3";
}