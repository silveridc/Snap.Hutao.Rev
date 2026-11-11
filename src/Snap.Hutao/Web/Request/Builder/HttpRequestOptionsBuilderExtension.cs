// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.
using Snap.Hutao.Rev.Core.Abstraction;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Diagnostics;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Request.Builder;

internal static class HttpRequestOptionsBuilderExtension
{
    extension<TBuilder>(TBuilder builder)
        where TBuilder : class, IHttpRequestOptionsBuilder
    {
        [DebuggerStepThrough]
        public TBuilder SetOptions<TValue>(HttpRequestOptionsKey<TValue> key, TValue value)
        {
            return builder.Configure(builder => builder.Options.Set(key, value));
        }
    }
}