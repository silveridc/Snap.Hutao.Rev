// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Update;

internal interface IUpdateService
{
    string? UpdateInfo { get; }

    ValueTask<CheckUpdateResult> CheckUpdateAsync(CancellationToken token = default);

    ValueTask TriggerUpdateAsync(CheckUpdateResult result, CancellationToken token = default);
}