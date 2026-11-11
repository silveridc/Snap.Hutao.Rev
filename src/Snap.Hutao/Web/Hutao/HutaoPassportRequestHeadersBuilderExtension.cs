// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Net.Http.Headers;

namespace Snap.Hutao.Rev.Web.Hutao;

internal static class HutaoPassportRequestHeadersBuilderExtension
{
    extension<TBuilder>(TBuilder builder)
        where TBuilder : IHttpHeadersBuilder<HttpRequestHeaders>
    {
        public TBuilder SetAccessToken(string? accessToken)
        {
            builder.Headers.Authorization = string.IsNullOrEmpty(accessToken) ? default : new("Bearer", accessToken);
            return builder;
        }

        public TBuilder SetHomaToken(string? homaToken)
        {
            if (!string.IsNullOrEmpty(homaToken))
            {
                builder.Headers.Add("x-homa-token", homaToken);
            }

            return builder;
        }
    }
}