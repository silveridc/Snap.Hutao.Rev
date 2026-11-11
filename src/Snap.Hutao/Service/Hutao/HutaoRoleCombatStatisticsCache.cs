// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.ViewModel.RoleCombat;
using Snap.Hutao.Rev.Web.Hutao.RoleCombat;
using System.Collections.Immutable;
using AvatarView = Snap.Hutao.Rev.ViewModel.Complex.AvatarView;

namespace Snap.Hutao.Rev.Service.Hutao;

[Service(ServiceLifetime.Singleton, typeof(IHutaoRoleCombatStatisticsCache))]
internal sealed partial class HutaoRoleCombatStatisticsCache : StatisticsCache, IHutaoRoleCombatStatisticsCache
{
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial HutaoRoleCombatStatisticsCache(IServiceProvider serviceProvider);

    public int RecordTotal { get; private set; }

    public ImmutableArray<AvatarView> AvatarAppearances { get; private set; }

    public async ValueTask InitializeForRoleCombatViewAsync(HutaoRoleCombatStatisticsMetadataContext context)
    {
        await InitializeForTypeAsync<RoleCombatViewModel, HutaoRoleCombatStatisticsMetadataContext>(context, AvatarAppearancesAsync).ConfigureAwait(false);
    }

    [SuppressMessage("", "SH003")]
    private async Task AvatarAppearancesAsync(HutaoRoleCombatStatisticsMetadataContext context)
    {
        RoleCombatStatisticsItem raw;
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            IHutaoRoleCombatService hutaoService = scope.ServiceProvider.GetRequiredService<IHutaoRoleCombatService>();
            raw = await hutaoService.GetRoleCombatStatisticsItemAsync().ConfigureAwait(false);
        }

        RecordTotal = raw.RecordTotal;
        AvatarAppearances = [.. CurrentJoinLast(raw.BackupAvatarRates.EmptyIfDefault().OrderByDescending(ir => ir.Rate), default, data => data.Item, (data, dataLast) => new AvatarView(context.GetAvatar(data.Item), data.Rate, dataLast?.Rate))];
    }
}