// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.RoleCombat;
using Snap.Hutao.Rev.ViewModel.User;
using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.Service.RoleCombat;

internal interface IRoleCombatService
{
    ValueTask<ObservableCollection<RoleCombatView>> GetRoleCombatViewCollectionAsync(RoleCombatMetadataContext context, UserAndUid userAndUid);

    ValueTask RefreshRoleCombatAsync(RoleCombatMetadataContext context, UserAndUid userAndUid);
}