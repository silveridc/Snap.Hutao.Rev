// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Web.Hutao.Response;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Snap.Hutao.Rev.Web.Hutao;

[HttpClient(HttpClientConfiguration.Default)]
internal sealed partial class HutaoPassportClient
{
    private const string PublicKey = """
        -----BEGIN PUBLIC KEY-----
        MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0Uv7IhpG+gBdwz8PFs0J
        GxblkYV+KIc8Yv1iNpXmT8mxLfNIStsn3ypSWlsdfikVU1CoPNGQUQWy6RAq1/SD
        OtHxUR1b/2E4HgPiGSwQRNnvdmi9FcgQx0pc6baNuHaF1HDSQRPh4a/LrJ2qGnMw
        s0CZFNsFvXD0FbmJHfxJTZc+V+CZW28OLLeD576HuJO2LI6pHWj7yFjTxYKj2jfY
        q9PdjsJmOtMZwmazYhrWuiuoXriFmAMQ+MxcqRAUZGHbkMh3qUJJJi6baB99/xFK
        aStB7P/8FFy8P1zbKGNanZtZER6Ji7YmIikllS6hi4WUcXu0314ZDFu4K2ephJzu
        dwIDAQAB
        -----END PUBLIC KEY-----
        """;

    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    private readonly IHutaoEndpointsFactory hutaoEndpointsFactory;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial HutaoPassportClient(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<HutaoResponse> RequestVerifyAsync(string email, VerifyCodeRequestType requestType, CancellationToken token = default)
    {
        Dictionary<string, object> data = new()
        {
            ["UserName"] = Encrypt(email),
        };

        if (requestType.HasFlag(VerifyCodeRequestType.ResetPassword))
        {
            data["IsResetPassword"] = true;
        }

        if (requestType.HasFlag(VerifyCodeRequestType.CancelRegistration))
        {
            data["IsCancelRegistration"] = true;
        }

        if (requestType.HasFlag(VerifyCodeRequestType.ResetUserName))
        {
            data["IsResetUserName"] = true;
        }

        if (requestType.HasFlag(VerifyCodeRequestType.ResetUserNameNew))
        {
            data["IsResetUserNameNew"] = true;
        }

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportVerificationCodes())
            .PostJson(data);

        HutaoResponse? resp = await builder
            .SendAsync<HutaoResponse>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse<TokenSet>> RegisterAsync(string email, string password, string verifyCode, CancellationToken token = default)
    {
        Dictionary<string, string> data = new()
        {
            ["UserName"] = Encrypt(email),
            ["Password"] = Encrypt(password),
            ["VerifyCode"] = Encrypt(verifyCode),
        };

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportAccounts())
            .PostJson(data);

        HutaoResponse<TokenSet>? resp = await builder
            .SendAsync<HutaoResponse<TokenSet>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse> UnregisterAsync(string? accessToken, string email, string password, string verifyCode, CancellationToken token = default)
    {
        Dictionary<string, string> data = new()
        {
            ["UserName"] = Encrypt(email),
            ["Password"] = Encrypt(password),
            ["VerifyCode"] = Encrypt(verifyCode),
        };

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportAccountMe())
            .SetAccessToken(accessToken)
            .Delete()
            .SetJsonContent(data);

        HutaoResponse? resp = await builder
            .SendAsync<HutaoResponse>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse<TokenSet>> ResetUserNameAsync(string email, string newUserName, string verifyCode, string newVerifyCode, CancellationToken token = default)
    {
        Dictionary<string, string> data = new()
        {
            ["UserName"] = Encrypt(email),
            ["NewUserName"] = Encrypt(newUserName),
            ["VerifyCode"] = Encrypt(verifyCode),
            ["NewVerifyCode"] = Encrypt(newVerifyCode),
        };

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportAccountMeUserName())
            .PutJson(data);

        HutaoResponse<TokenSet>? resp = await builder
            .SendAsync<HutaoResponse<TokenSet>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse<TokenSet>> LoginAsync(string email, string password, string? captchaToken, CancellationToken token = default)
    {
        Dictionary<string, string> data = new()
        {
            ["UserName"] = Encrypt(email),
            ["Password"] = Encrypt(password),
        };

        if (!string.IsNullOrEmpty(captchaToken))
        {
            data["CaptchaToken"] = captchaToken;
        }

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportSessions())
            .PostJson(data);

        HutaoResponse<TokenSet>? resp = await builder
            .SendAsync<HutaoResponse<TokenSet>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    /// <summary>
    /// 获取登录页的人机验证配置。
    /// </summary>
    /// <param name="token">取消令牌</param>
    /// <returns>是否启用与站点密钥。</returns>
    public async ValueTask<HutaoResponse<PassportCaptchaSettings>> GetCaptchaSettingsAsync(CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportCaptcha())
            .Get();

        HutaoResponse<PassportCaptchaSettings>? resp = await builder
            .SendAsync<HutaoResponse<PassportCaptchaSettings>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse<UserInfo>> GetUserInfoAsync(string? accessToken, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportMe())
            .SetAccessToken(accessToken)
            .Get();

        HutaoResponse<UserInfo>? resp = await builder
            .SendAsync<HutaoResponse<UserInfo>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse<AccountLoginLogPage>> GetLoginLogsAsync(string? accessToken, int page, int pageSize, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportLoginLog(page, pageSize))
            .SetAccessToken(accessToken)
            .Get();

        HutaoResponse<AccountLoginLogPage>? resp = await builder
            .SendAsync<HutaoResponse<AccountLoginLogPage>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse<TokenSet>> RefreshTokenAsync(string refreshToken, CancellationToken token = default)
    {
        Dictionary<string, string> data = new()
        {
            ["RefreshToken"] = Encrypt(refreshToken),
        };

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportSessionRefresh())
            .PostJson(data);

        HutaoResponse<TokenSet>? resp = await builder
            .SendAsync<HutaoResponse<TokenSet>>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse> RevokeTokenAsync(string? accessToken, string deviceId, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportSessions(Encrypt(deviceId)))
            .SetAccessToken(accessToken)
            .Delete();

        HutaoResponse? resp = await builder
            .SendAsync<HutaoResponse>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    public async ValueTask<HutaoResponse> RevokeAllTokensAsync(string? accessToken, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(hutaoEndpointsFactory.Create().PassportSessions())
            .SetAccessToken(accessToken)
            .Delete();

        HutaoResponse? resp = await builder
            .SendAsync<HutaoResponse>(httpClient, token)
            .ConfigureAwait(false);

        return Web.Response.Response.DefaultIfNull(resp);
    }

    private static string Encrypt(string text)
    {
        using (RSA rsa = RSA.Create(2048))
        {
            rsa.ImportFromPem(PublicKey);
            byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.OaepSHA1);
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}