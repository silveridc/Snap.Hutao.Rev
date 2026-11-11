// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Service.Achievement;
using Snap.Hutao.Rev.Service.Metadata;
using Snap.Hutao.Rev.Service.Metadata.ContextAbstraction;
using Snap.Hutao.Rev.UI.Xaml.View.Page;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.ViewModel.Achievement;

[BindableCustomPropertyProvider]
[Service(ServiceLifetime.Transient)]
internal sealed partial class AchievementViewModelSlim : Abstraction.ViewModelSlim<AchievementPage>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial AchievementViewModelSlim(IServiceProvider serviceProvider);

    [ObservableProperty]
    public partial ImmutableArray<AchievementStatistics> StatisticsList { get; set; } = [];

    protected override async Task LoadAsync()
    {
        using (IServiceScope scope = ServiceProvider.CreateScope())
        {
            ITaskContext taskContext = scope.ServiceProvider.GetRequiredService<ITaskContext>();
            IMetadataService metadataService = scope.ServiceProvider.GetRequiredService<IMetadataService>();

            if (!await metadataService.InitializeAsync().ConfigureAwait(false))
            {
                return;
            }

            AchievementServiceMetadataContext context = await metadataService
                .GetContextAsync<AchievementServiceMetadataContext>()
                .ConfigureAwait(false);
            ImmutableArray<AchievementStatistics> array = await scope.ServiceProvider
                .GetRequiredService<IAchievementStatisticsService>()
                .GetAchievementStatisticsAsync(context)
                .ConfigureAwait(false);

            await taskContext.SwitchToMainThreadAsync();
            StatisticsList = array;
            IsInitialized = true;
        }
    }
}