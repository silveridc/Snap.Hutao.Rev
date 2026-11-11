// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab;

[Flags]
internal enum CookieType
{
    None = 0B0000,
    CookieToken = 0B0001,
    LToken = 0B0010,
    Cookie = CookieToken | LToken,
    SToken = 0B0100,
    All = 0B0111,
}