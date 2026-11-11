// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Metadata;

internal interface IMetadataServiceInitialization
{
    ValueTask InitializeInternalAsync(CancellationToken token = default);
}