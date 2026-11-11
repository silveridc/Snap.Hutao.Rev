// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Request.Builder.Abstraction;

internal interface IHttpMethodBuilder : IBuilder
{
    HttpMethod Method { get; set; }
}