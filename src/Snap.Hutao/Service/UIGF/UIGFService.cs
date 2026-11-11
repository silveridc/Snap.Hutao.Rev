// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.UIGF;

[Service(ServiceLifetime.Singleton, typeof(IUIGFService))]
internal sealed partial class UIGFService : IUIGFService
{
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial UIGFService(IServiceProvider serviceProvider);

    public async ValueTask ExportAsync(UIGFExportOptions exportOptions, CancellationToken token = default)
    {
        IUIGFExportService exportService = serviceProvider.GetRequiredKeyedService<IUIGFExportService>(UIGFVersion.UIGF41);
        await exportService.ExportAsync(exportOptions, token).ConfigureAwait(false);
    }

    public async ValueTask ImportAsync(UIGFImportOptions importOptions, CancellationToken token = default)
    {
        UIGFVersion version = importOptions.UIGF.Info.Version switch
        {
            "v4.0" => UIGFVersion.UIGF40,
            "v4.1" => UIGFVersion.UIGF41,
            _ => UIGFVersion.None,
        };

        IUIGFImportService importService = serviceProvider.GetRequiredKeyedService<IUIGFImportService>(version);
        await importService.ImportAsync(importOptions, token).ConfigureAwait(false);
    }
}