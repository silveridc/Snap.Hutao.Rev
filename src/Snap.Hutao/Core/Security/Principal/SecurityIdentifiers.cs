// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Security.Principal;

namespace Snap.Hutao.Rev.Core.Security.Principal;

internal static class SecurityIdentifiers
{
    public static SecurityIdentifier Everyone { get; } = new(WellKnownSidType.WorldSid, null);
}