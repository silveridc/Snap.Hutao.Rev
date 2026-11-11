// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaPassportEndpoints : IHomaRootAccess
{
    string PassportVerificationCodes()
    {
        return $"{Root}/passport/v2/verification-codes";
    }

    string PassportAccounts()
    {
        return $"{Root}/passport/v2/accounts";
    }

    string PassportAccountMe()
    {
        return $"{Root}/passport/v2/accounts/me";
    }

    string PassportAccountMeUserName()
    {
        return $"{Root}/passport/v2/accounts/me/username";
    }

    string PassportSessions(string? encryptedDeviceId = null)
    {
        return string.IsNullOrEmpty(encryptedDeviceId)
            ? $"{Root}/passport/v2/sessions"
            : $"{Root}/passport/v2/sessions?deviceId={Uri.EscapeDataString(encryptedDeviceId)}";
    }

    string PassportSessionRefresh()
    {
        return $"{Root}/passport/v2/sessions/refresh";
    }

    string PassportMe()
    {
        return $"{Root}/passport/v2/me";
    }

    string PassportLoginLog(int page, int pageSize)
    {
        return $"{Root}/passport/v2/me/login-logs?page={page}&pageSize={pageSize}";
    }

    string PassportCaptcha()
    {
        return $"{Root}/passport/v2/captcha";
    }

    string PassportCaptchaPage()
    {
        return $"{Root}/passport/v2/captcha/page";
    }
}
