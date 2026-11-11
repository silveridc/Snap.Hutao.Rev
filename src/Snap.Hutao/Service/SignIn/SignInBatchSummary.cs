// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.SignIn;

internal sealed record SignInBatchSummary(int Claimed, int AlreadySigned, ImmutableArray<string> Failures);
