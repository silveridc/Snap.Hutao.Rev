// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.User;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.SignIn;

internal interface ISignInService
{
    ValueTask<bool> ClaimSignInRewardAsync(UserAndUid userAndUid, CancellationToken token = default);

    ValueTask<bool> ClaimResignInRewardAsync(UserAndUid userAndUid, CancellationToken token = default);

    ValueTask<SignInBatchSummary> ClaimAllSignInRewardAsync(IImmutableList<UserAndUid> targets, CancellationToken token = default);
}