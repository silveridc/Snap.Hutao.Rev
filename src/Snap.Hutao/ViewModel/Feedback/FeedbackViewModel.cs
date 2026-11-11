// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.DataTransfer;
using Snap.Hutao.Rev.Core.IO.Http.Loopback;
using Snap.Hutao.Rev.Core.IO.Http.Proxy;
using Snap.Hutao.Rev.Core.LifeCycle;
using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.Service;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.UI.Xaml.Behavior.Action;
using Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;
using Snap.Hutao.Rev.Web.Hutao;
using Snap.Hutao.Rev.Web.Hutao.Algolia;
using System.Runtime.InteropServices;
using Windows.System;

namespace Snap.Hutao.Rev.ViewModel.Feedback;

[BindableCustomPropertyProvider]
[Service(ServiceLifetime.Scoped)]
internal sealed partial class FeedbackViewModel : Abstraction.ViewModel
{
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly IClipboardProvider clipboardProvider;
    private readonly IServiceProvider serviceProvider;
    private readonly CultureOptions cultureOptions;
    private readonly ITaskContext taskContext;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial FeedbackViewModel(IServiceProvider serviceProvider);

    public static HttpProxyUsingSystemProxy DynamicHttpProxy { get => HttpProxyUsingSystemProxy.Instance; }

    public partial RuntimeOptions RuntimeOptions { get; }

    public partial LoopbackSupport LoopbackSupport { get; }

    [ObservableProperty]
    public partial string? SearchText { get; set; }

    [ObservableProperty]
    public partial List<AlgoliaHit>? SearchResults { get; set; }

    [ObservableProperty]
    public partial IPInformation? IPInformation { get; private set; }

    protected override async ValueTask<bool> LoadOverrideAsync(CancellationToken token)
    {
        IPInformation? info;
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            HutaoInfrastructureClient hutaoInfrastructureClient = scope.ServiceProvider.GetRequiredService<HutaoInfrastructureClient>();
            info = await hutaoInfrastructureClient.GetIPInformationAsync(token).ConfigureAwait(false);
        }

        info ??= IPInformation.Default;
        await taskContext.SwitchToMainThreadAsync();
        IPInformation = info;

        return true;
    }

    [Command("NavigateToUriCommand")]
    private async Task NavigateToUri(string? uri)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateUI("Navigate to uri", "FeedbackViewModel.Command", [("uri", uri ?? "<null>")]));

        if (string.IsNullOrEmpty(uri))
        {
            return;
        }

        Uri targetUri = uri.ToUri();
        if (targetUri.Host.Equals("hut.ao", StringComparison.OrdinalIgnoreCase))
        {
            NavigateToDocument(targetUri.PathAndQuery.TrimStart('/'));
            return;
        }

        await Launcher.LaunchUriAsync(targetUri);
    }

    [Command("NavigateToDocumentCommand")]
    private void NavigateToDocument(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            return;
        }

        ICurrentXamlWindowReference currentWindow = serviceProvider.GetRequiredService<ICurrentXamlWindowReference>();
        if (currentWindow.XamlRoot is { } xamlRoot)
        {
            ShowWebView2WindowAction.Show(new RemoteDocumentationContentProvider(relativePath), xamlRoot);
        }
    }

    [Command("SearchDocumentCommand")]
    private async Task SearchDocumentAsync(string? search)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateUI("Search", "FeedbackViewModel.Command", [("text", search ?? "<null>")]));

        IsInitialized = false;
        SearchResults = null;

        if (string.IsNullOrEmpty(search))
        {
            IsInitialized = true;
            return;
        }

        string language = LocaleNames.GetLanguageCodeForDocumentationSearchFromLocaleName(cultureOptions.LocaleName);
        AlgoliaResponse? response;
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            HutaoDocumentationClient hutaoDocumentationClient = scope.ServiceProvider.GetRequiredService<HutaoDocumentationClient>();
            response = await hutaoDocumentationClient.QueryAsync(search, language).ConfigureAwait(false);
        }

        await taskContext.SwitchToMainThreadAsync();
        if (response is { Results: [{ Hits: { Count: > 0 } hits }, ..] })
        {
            SearchResults = [.. hits.DistinctBy(hit => hit.Url)];
        }
        else
        {
            SearchResults = null;
        }

        IsInitialized = true;
    }

    [Command("CopyDeviceIdCommand")]
    private async Task CopyDeviceIdAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Copy device id", "FeedbackViewModel.Command"));

        try
        {
            await clipboardProvider.SetTextAsync(HutaoRuntime.DeviceId).ConfigureAwait(false);
            messenger.Send(InfoBarMessage.Success(SH.ViewModelSettingCopyDeviceIdSuccess));
        }
        catch (COMException ex)
        {
            messenger.Send(InfoBarMessage.Error(ex));
        }
    }

    [Command("EnableLoopbackCommand")]
    private async Task EnableLoopbackAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Enable Loopback", "FeedbackViewModel.Command"));

        ContentDialogResult result = await contentDialogFactory
            .CreateForConfirmCancelAsync(SH.ViewDialogFeedbackEnableLoopbackTitle, SH.ViewDialogFeedbackEnableLoopbackContent)
            .ConfigureAwait(false);

        if (result is ContentDialogResult.Primary)
        {
            await taskContext.SwitchToMainThreadAsync();
            LoopbackSupport.EnableLoopback();
        }
    }
}