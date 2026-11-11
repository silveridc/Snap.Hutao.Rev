// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;
using System.Net.Http.Headers;

namespace Snap.Hutao.Rev.Web.Request.Builder.Abstraction;

internal interface IHttpHeadersBuilder<out T> : IBuilder
    where T : HttpHeaders
{
    T Headers { get; }
}