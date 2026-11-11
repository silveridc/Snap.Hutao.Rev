// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model.Entity;
using Snap.Hutao.Rev.ViewModel.User;
using System.Collections.ObjectModel;

namespace Snap.Hutao.Rev.Service.DailyNote;

internal interface IDailyNoteService
{
    ValueTask AddDailyNoteAsync(DailyNoteMetadataContext context, UserAndUid userAndUid, CancellationToken token = default);

    ValueTask<ObservableCollection<DailyNoteEntry>> GetDailyNoteEntryCollectionAsync(DailyNoteMetadataContext context, bool forceRefresh = false, CancellationToken token = default);

    ValueTask RefreshDailyNotesAsync(CancellationToken token = default);

    ValueTask RemoveDailyNoteAsync(DailyNoteEntry entry, CancellationToken token = default);

    ValueTask<bool> UpdateDailyNoteAsync(DailyNoteEntry entry, CancellationToken token = default);
}