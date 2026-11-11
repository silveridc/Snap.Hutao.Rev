// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hoyolab;

internal static class ApiEndpointsFactoryExtension
{
    extension(IApiEndpointsFactory factory)
    {
        public IApiEndpoints Create(bool isOversea)
        {
            return factory.Create(isOversea ? ApiEndpointsKind.Oversea : ApiEndpointsKind.Chinese);
        }
    }
}