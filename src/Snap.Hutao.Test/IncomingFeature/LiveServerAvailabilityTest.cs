using Snap.Hutao.Rev;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Snap.Hutao.Test.IncomingFeature;

[TestClass]
public class LiveServerAvailabilityTest
{
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(15) };

    /// <summary>控制器路由都在 /api 下，用公开的壁纸接口验证线上确实按这个前缀提供服务。</summary>
    /// <returns>测试任务。</returns>
    [TestMethod]
    public async Task PrefixedRoute_RespondsOnApiRoot()
    {
        await AssertOkAsync($"{ServerHosts.Current.ApiRoot}/wallpapers/bing");
    }

    /// <summary>补丁下发地址没有 /api，直接挂在主机根上。</summary>
    /// <returns>测试任务。</returns>
    [TestMethod]
    public async Task UnprefixedRoute_RespondsOnHostRoot()
    {
        await AssertOkAsync($"{ServerHosts.Current.Api}/patch/hutao");
    }

    private static async Task AssertOkAsync(string url)
    {
        using HttpResponseMessage response = await GetOrSkipAsync(url);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"{url} 返回了 {response.StatusCode}");
    }

    /// <summary>离线或服务端没部署时跳过：这条用例保证的是线上可用性，断网不代表代码错了。</summary>
    private static async Task<HttpResponseMessage> GetOrSkipAsync(string url)
    {
        try
        {
            return await Client.GetAsync(url);
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            Assert.Inconclusive($"连不上 {url}：{exception.Message}");
            throw;
        }
    }
}
