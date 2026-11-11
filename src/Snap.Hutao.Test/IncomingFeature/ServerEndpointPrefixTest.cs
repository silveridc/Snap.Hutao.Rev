using Snap.Hutao.Rev;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snap.Hutao.Test.IncomingFeature;

[TestClass]
public class ServerEndpointPrefixTest
{
    /// <summary>
    /// 与被测接口同源的探针：Root 的取法与 HutaoEndpointsForRelease 一致，
    /// 断言的是真实源码里的拼接规则，而不是抄一份地址。
    /// </summary>
    private sealed class EndpointProbe : IHomaRootAccess, IInfrastructureRootAccess, IInfrastructureRawRootAccess,
        IHomaServiceEndpoints, IHomaPassportEndpoints, IInfrastructureFeatureEndpoints,
        IInfrastructurePatchEndpoints, IInfrastructureManagementEndpoints, IInfrastructureGitRepositoryEndpoints
    {
        string IHomaRootAccess.Root => ServerHosts.Current.ApiRoot;

        string IInfrastructureRootAccess.Root => ServerHosts.Current.Infrastructure;

        string IInfrastructureRawRootAccess.RawRoot => ServerHosts.Current.Infrastructure;
    }

    [TestMethod]
    public void ApiRoot_AppendsTheApiLayerExactlyOnce()
    {
        foreach (ServerHosts.Host host in new[] { ServerHosts.Production, ServerHosts.Test })
        {
            Assert.AreEqual($"{host.Api}/api", host.ApiRoot, $"ApiRoot 的规则变了：{host.ApiRoot}。改之前先确认服务端路由。");
            Assert.IsFalse(host.Api.EndsWith("/api", StringComparison.OrdinalIgnoreCase), $"Host.Api 不该自带 /api：{host.Api}");
            Assert.IsFalse(host.Api.EndsWith('/'), $"Host.Api 不该以 / 结尾：{host.Api}");
            Assert.IsFalse(host.Infrastructure.EndsWith('/'), $"Host.Infrastructure 不该以 / 结尾：{host.Infrastructure}");
        }
    }

    /// <summary>控制器路由都在 /api 下：这里曾经拼出 /api/api/Announcement/List 导致 404。</summary>
    [TestMethod]
    public void ControllerEndpoints_UseThePrefixedRoot()
    {
        EndpointProbe probe = new();
        IHomaServiceEndpoints service = probe;
        IHomaPassportEndpoints passport = probe;
        string api = ServerHosts.Current.Api;

        Assert.AreEqual($"{api}/api/announcements?locale=zh-cn&exclude=", service.Announcement("zh-cn", []));
        Assert.AreEqual($"{api}/api/admin/announcements", service.AnnouncementUpload());
        Assert.AreEqual($"{api}/api/admin/redeem-codes/generation", service.RedeemCodeGenerate());
        Assert.AreEqual($"{api}/api/passport/v2/me", passport.PassportMe());
        Assert.AreEqual($"{api}/api/passport/v2/sessions", passport.PassportSessions());
        Assert.AreEqual($"{api}/api/passport/v2/sessions?deviceId=device", passport.PassportSessions("device"));
    }

    /// <summary>/patch/*、/mgnt/*、/git-repository/* 在服务端没有包 /api。</summary>
    [TestMethod]
    public void UnprefixedEndpoints_SkipTheApiLayer()
    {
        EndpointProbe probe = new();
        IHomaPassportEndpoints passport = probe;
        IInfrastructurePatchEndpoints patch = probe;
        IInfrastructureManagementEndpoints management = probe;
        IInfrastructureGitRepositoryEndpoints gitRepository = probe;
        string api = ServerHosts.Current.Api;

        Assert.AreEqual($"{api}/patch/hutao", patch.PatchSnapHutao());
        Assert.AreEqual($"{api}/patch/yae", patch.PatchYaeAchievement());
        Assert.AreEqual($"{api}/mgnt/am-i-banned", management.AmIBanned());
        Assert.AreEqual($"{api}/git-repository/all?name=Snap.Hutao", gitRepository.GitRepository("Snap.Hutao"));
    }

    /// <summary>功能开关配置由客户端从接口域名拉取，所以走带前缀的根。</summary>
    [TestMethod]
    public void FeatureSwitchJson_LivesUnderTheApiRoot()
    {
        EndpointProbe probe = new();
        IInfrastructureFeatureEndpoints feature = probe;

        Assert.AreEqual($"{ServerHosts.Current.Api}/api/client/features/FpsUnlock", feature.Feature("FpsUnlock"));
    }

    [TestMethod]
    public void NoEndpoint_DoublesTheApiLayer()
    {
        EndpointProbe probe = new();
        IHomaServiceEndpoints service = probe;
        IHomaPassportEndpoints passport = probe;
        IInfrastructureFeatureEndpoints feature = probe;
        IInfrastructurePatchEndpoints patch = probe;
        IInfrastructureManagementEndpoints management = probe;
        IInfrastructureGitRepositoryEndpoints gitRepository = probe;

        List<string> urls =
        [
            service.Announcement("zh-cn", []),
            service.AnnouncementUpload(),
            service.GachaLogCompensation(1),
            service.GachaLogDesignation("user", 1),
            service.CdnCompensation(1),
            service.CdnDesignation("user", 1),
            service.RedeemCodeGenerate(),
            passport.PassportVerificationCodes(),
            passport.PassportAccounts(),
            passport.PassportAccountMe(),
            passport.PassportAccountMeUserName(),
            passport.PassportSessions(),
            passport.PassportMe(),
            passport.PassportSessionRefresh(),
            passport.PassportSessions("device"),
            passport.PassportSessions(),
            feature.Feature("FpsUnlock"),
            patch.PatchSnapHutao(),
            patch.PatchYaeAchievement(),
            management.AmIBanned(),
            gitRepository.GitRepository("Snap.Hutao"),
        ];

        string[] doubled = [.. urls.Where(url => url.Contains("/api/api", StringComparison.OrdinalIgnoreCase))];
        Assert.AreEqual(0, doubled.Length, $"出现重复的 /api 前缀：\n{string.Join('\n', doubled)}");

        string[] stray = [.. urls.Where(url => !url.StartsWith(ServerHosts.Current.Api, StringComparison.Ordinal) && !url.StartsWith(ServerHosts.Current.Infrastructure, StringComparison.Ordinal))];
        Assert.AreEqual(0, stray.Length, $"端点没有走 ServerHosts 的根地址：\n{string.Join('\n', stray)}");
    }
}
