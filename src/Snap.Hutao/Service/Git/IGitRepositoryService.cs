// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.IO;

namespace Snap.Hutao.Rev.Service.Git;

internal interface IGitRepositoryService
{
    ValueTask<ValueResult<bool, ValueDirectory>> EnsureRepositoryAsync(string name);
}