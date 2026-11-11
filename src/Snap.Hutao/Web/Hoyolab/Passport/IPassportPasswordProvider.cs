// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.Passport;

internal interface IPassportPasswordProvider : IAigisProvider, IVerifyProvider
{
    string? Account { get; }

    string? Password { get; }
}