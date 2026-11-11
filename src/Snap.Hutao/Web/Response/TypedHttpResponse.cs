// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Abstraction;
using System.Net.Http.Headers;

namespace Snap.Hutao.Rev.Web.Response;

internal sealed class TypedHttpResponse<TResult> : IDeconstruct<HttpResponseHeaders?, TResult?>
    where TResult : class
{
    public TypedHttpResponse(HttpResponseHeaders? headers, TResult? result)
    {
        Headers = headers;
        Body = result;
    }

    public HttpResponseHeaders? Headers { get; }

    public TResult? Body { get; }

    public static implicit operator TResult?(TypedHttpResponse<TResult> response)
    {
        return response.Body;
    }

    public void Deconstruct(out HttpResponseHeaders? headers, out TResult? body)
    {
        headers = Headers;
        body = Body;
    }
}