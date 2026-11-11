// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hutao;

[Flags]
internal enum VerifyCodeRequestType
{
    Registration = 0b0000,
    ResetPassword = 0b0001,
    CancelRegistration = 0b0010,
    ResetUserName = 0b0100,
    ResetUserNameNew = 0b1000,
}