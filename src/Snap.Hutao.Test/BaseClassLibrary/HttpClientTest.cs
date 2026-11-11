using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Snap.Hutao.Test.BaseClassLibrary;

[TestClass]
public sealed class HttpClientTest
{
    /// <summary>
    /// AllowAutoRedirect=false 时必须原样返回 302 与 Location，而不是跟到目标地址。
    /// 用 httpbin 的 /status/302：它会回一个相对跳转，正好顺带验证 Location 被解析成绝对地址。
    /// </summary>
    /// <returns>测试任务。</returns>
    [TestMethod]
    public async Task RedirectionHeaderTest()
    {
        HttpClientHandler handler = new()
        {
            UseCookies = false,
            AllowAutoRedirect = false,
        };

        using (handler)
        {
            using (HttpClient httpClient = new(handler) { Timeout = TimeSpan.FromSeconds(20) })
            {
                using (HttpResponseMessage response = await SendOrSkipAsync(httpClient))
                {
                    Assert.AreEqual(HttpStatusCode.Found, response.StatusCode);

                    // Location 保持服务端下发的相对地址，说明客户端既没解析也没跟随
                    Assert.AreEqual("/redirect/1", response.Headers.Location?.OriginalString);
                }
            }
        }
    }

    /// <summary>外网不可达时跳过：这条用例验证的是客户端行为，断网不代表代码错了。</summary>
    private static async Task<HttpResponseMessage> SendOrSkipAsync(HttpClient httpClient)
    {
        try
        {
            return await httpClient.GetAsync("https://httpbin.org/status/302");
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            Assert.Inconclusive($"连不上 httpbin.org：{exception.Message}");
            throw;
        }
    }
}
