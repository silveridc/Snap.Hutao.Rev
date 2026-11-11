// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

internal interface IPassportMobileCaptchaProvider : IAigisProvider
{
    string? ActionType { get; }

    string? Mobile { get; }

    string? Captcha { get; }
}