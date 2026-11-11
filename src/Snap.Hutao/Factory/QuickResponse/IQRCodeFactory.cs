// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Factory.QuickResponse;

internal interface IQRCodeFactory
{
    byte[] Create(string source);
}