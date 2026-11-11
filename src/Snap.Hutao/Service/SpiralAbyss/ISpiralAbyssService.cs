// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.SpiralAbyss;
using Snap.Hutao.Rev.ViewModel.User;
using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.Service.SpiralAbyss;

internal interface ISpiralAbyssService
{
    ValueTask<ObservableCollection<SpiralAbyssView>> GetSpiralAbyssViewCollectionAsync(SpiralAbyssMetadataContext context, UserAndUid userAndUid);

    ValueTask RefreshSpiralAbyssAsync(SpiralAbyssMetadataContext context, UserAndUid userAndUid);
}