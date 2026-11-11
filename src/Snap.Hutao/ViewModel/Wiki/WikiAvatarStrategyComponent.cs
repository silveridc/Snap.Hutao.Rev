// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Model.Metadata.Avatar;
using Snap.Hutao.Rev.Service;
using Snap.Hutao.Rev.Service.Hutao;
using Windows.System;

namespace Snap.Hutao.Rev.ViewModel.Wiki;

[Service(ServiceLifetime.Scoped)]
internal sealed partial class WikiAvatarStrategyComponent
{
    private readonly IAvatarStrategyService avatarStrategyService;
    private readonly CultureOptions cultureOptions;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial WikiAvatarStrategyComponent(IServiceProvider serviceProvider);

    public bool IsBilibiliAvailable { get => cultureOptions.LocaleName is LocaleNames.CHS; }

    [Command("BilibiliStrategyCommand")]
    private static async Task OpenBilibiliStrategyWebsiteAsync(Avatar? avatar)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateUI("Open strategy website", "WikiAvatarStrategyComponent.Command", [("target", "bilibili")]));

        if (avatar is null)
        {
            return;
        }

        Uri targetUri = $"https://wiki.biligame.com/ys/{avatar.Name}/攻略".ToUri();
        await Launcher.LaunchUriAsync(targetUri);
    }

    // 以下两个攻略入口依赖胡桃官方后端（/strategy/item），官方删库后接口已失效。
    // 改为统一使用 Bilibili Wiki 攻略，故注释保留，后续如需恢复再放开。
    // [Command("ChineseStrategyCommand")]
    // private async Task OpenChineseStrategyWebsiteAsync(Avatar? avatar)
    // {
    //    SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateUI("Open strategy website", "WikiAvatarStrategyComponent.Command", [("target", "miyoushe")]));
    //
    //    if (avatar is null)
    //    {
    //        return;
    //    }
    //
    //    AvatarStrategy? strategy = await avatarStrategyService.GetStrategyByAvatarId(avatar.Id).ConfigureAwait(false);
    //
    //    if (strategy is null)
    //    {
    //        messenger.Send(InfoBarMessage.Warning(SH.ViewModelWikiAvatarStrategyNotFound));
    //        return;
    //    }
    //
    //    Uri targetUri = strategy.ChineseStrategyUrl;
    //    if (string.IsNullOrEmpty(targetUri.OriginalString))
    //    {
    //        messenger.Send(InfoBarMessage.Warning(SH.ViewModelWikiAvatarStrategyNotFound));
    //        return;
    //    }
    //
    //    await Launcher.LaunchUriAsync(targetUri);
    // }

    // [Command("OverseaStrategyCommand")]
    // private async Task OpenOverseaStrategyWebsiteAsync(Avatar? avatar)
    // {
    //    SentrySdk.AddBreadcrumb(BreadcrumbFactory2.CreateUI("Open strategy website", "WikiAvatarStrategyComponent.Command", [("target", "hoyolab")]));
    //
    //    if (avatar is null)
    //    {
    //        return;
    //    }
    //
    //    AvatarStrategy? strategy = await avatarStrategyService.GetStrategyByAvatarId(avatar.Id).ConfigureAwait(false);
    //
    //    if (strategy is null)
    //    {
    //        messenger.Send(InfoBarMessage.Warning(SH.ViewModelWikiAvatarStrategyNotFound));
    //        return;
    //    }
    //
    //    Uri targetUri = strategy.OverseaStrategyUrl;
    //    if (string.IsNullOrEmpty(targetUri.OriginalString))
    //    {
    //        messenger.Send(InfoBarMessage.Warning(SH.ViewModelWikiAvatarStrategyNotFound));
    //        return;
    //    }
    //
    //    await Launcher.LaunchUriAsync(targetUri);
    // }
}
