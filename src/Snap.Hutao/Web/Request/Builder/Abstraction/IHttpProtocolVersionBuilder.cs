// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;

namespace Snap.Hutao.Rev.Web.Request.Builder.Abstraction;

internal interface IHttpProtocolVersionBuilder : IBuilder
{
    Version Version { get; set; }
}