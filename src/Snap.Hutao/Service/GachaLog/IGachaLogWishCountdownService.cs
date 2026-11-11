// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.GachaLog;

namespace Snap.Hutao.Rev.Service.GachaLog;

internal interface IGachaLogWishCountdownService
{
    ValueTask<WishCountdownBundle> GetWishCountdownBundleAsync(GachaLogWishCountdownServiceMetadataContext context);
}