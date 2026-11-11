// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.User;

internal interface IUserFingerprintService
{
    ValueTask TryInitializeAsync(ViewModel.User.User user, CancellationToken token = default);
}