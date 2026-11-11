// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;
using Snap.Hutao.Rev.Core.LifeCycle;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.UI.Xaml.Behavior.Action;
using Snap.Hutao.Rev.UI.Xaml.View.Window.WebView2;
using Snap.Hutao.Rev.ViewModel.User;
using Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.BbsSignReward;
using Snap.Hutao.Rev.Web.Response;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.SignIn;

[Service(ServiceLifetime.Singleton, typeof(ISignInService))]
internal sealed partial class SignInService : ISignInService
{
    private readonly ICurrentXamlWindowReference currentXamlWindowReference;
    private readonly IServiceProvider serviceProvider;
    private readonly ITaskContext taskContext;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial SignInService(IServiceProvider serviceProvider);

    public async ValueTask<bool> ClaimSignInRewardAsync(UserAndUid userAndUid, CancellationToken token = default)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            ISignInClient signInClient = scope.ServiceProvider
                .GetRequiredService<IOverseaSupportFactory<ISignInClient>>()
                .Create(userAndUid.IsOversea);

            Response<SignInResult> resultResponse = await signInClient.SignAsync(userAndUid, token).ConfigureAwait(false);
            if (ResponseValidator.TryValidateWithoutUINotification(resultResponse, out SignInResult? result))
            {
                return true;
            }

            string message = resultResponse.Message;

            if (resultResponse.ReturnCode is (int)KnownReturnCode.AlreadySignedIn)
            {
                messenger.Send(InfoBarMessage.Success(message));
                return false;
            }

            if (string.IsNullOrEmpty(message))
            {
                message = $"RiskCode: {result?.RiskCode}";
            }

            messenger.Send(InfoBarMessage.Error(SH.FormatServiceSignInClaimRewardFailed(message)));
            await FallbackToWebView2SignInAsync().ConfigureAwait(false);
            return false;
        }
    }

    public async ValueTask<bool> ClaimResignInRewardAsync(UserAndUid userAndUid, CancellationToken token = default)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            ISignInClient signInClient = scope.ServiceProvider
                .GetRequiredService<IOverseaSupportFactory<ISignInClient>>()
                .Create(userAndUid.IsOversea);

            Response<SignInResult> resultResponse = await signInClient.ReSignAsync(userAndUid, token).ConfigureAwait(false);
            if (ResponseValidator.TryValidateWithoutUINotification(resultResponse, out SignInResult? signInResult))
            {
                return true;
            }

            string message = resultResponse.Message;

            if (resultResponse.ReturnCode is (int)KnownReturnCode.ResignQuotaUsedUp or (int)KnownReturnCode.PleaseSignInFirst or (int)KnownReturnCode.NoAvailableResignDate)
            {
                messenger.Send(InfoBarMessage.Error(message));
                return false;
            }

            if (resultResponse.ReturnCode is (int)KnownReturnCode.NotEnoughCoin)
            {
                message = SH.ViewModelSignInReSignInNotEnoughCoinMessage;
                messenger.Send(InfoBarMessage.Error(message));
                return false;
            }

            if (string.IsNullOrEmpty(message))
            {
                message = $"RiskCode: {signInResult?.RiskCode}";
            }

            messenger.Send(InfoBarMessage.Error(SH.FormatServiceReSignInClaimRewardFailed(message)));
            await FallbackToWebView2SignInAsync().ConfigureAwait(false);
            return false;
        }
    }

    public async ValueTask<SignInBatchSummary> ClaimAllSignInRewardAsync(IImmutableList<UserAndUid> targets, CancellationToken token = default)
    {
        int claimed = 0;
        int alreadySigned = 0;
        ImmutableArray<string>.Builder failures = ImmutableArray.CreateBuilder<string>();

        foreach (UserAndUid target in targets)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                ISignInClient signInClient = scope.ServiceProvider
                    .GetRequiredService<IOverseaSupportFactory<ISignInClient>>()
                    .Create(target.IsOversea);

                Response<SignInResult> resultResponse;
                try
                {
                    resultResponse = await signInClient.SignAsync(target, token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    failures.Add($"{target.Uid.Value}: {ex.Message}");
                    continue;
                }

                if (ResponseValidator.TryValidateWithoutUINotification(resultResponse, out _))
                {
                    claimed++;
                    continue;
                }

                if (resultResponse.ReturnCode is (int)KnownReturnCode.AlreadySignedIn)
                {
                    alreadySigned++;
                    continue;
                }

                string message = string.IsNullOrEmpty(resultResponse.Message) ? $"RiskCode: {resultResponse.Data?.RiskCode}" : resultResponse.Message;
                failures.Add($"{target.Uid.Value}: {message}");
            }
        }

        return new(claimed, alreadySigned, failures.ToImmutable());
    }

    private async ValueTask FallbackToWebView2SignInAsync()
    {
        await taskContext.SwitchToMainThreadAsync();

        if (currentXamlWindowReference.XamlRoot is not { } xamlRoot)
        {
            return;
        }

        MiHoYoJSBridgeWebView2ContentProvider provider = new()
        {
            SourceProvider = new SignInJSBridgeUriSourceProvider(),
        };

        ShowWebView2WindowAction.Show(provider, xamlRoot);
    }
}