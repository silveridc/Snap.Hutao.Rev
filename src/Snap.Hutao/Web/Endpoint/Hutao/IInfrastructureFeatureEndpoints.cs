// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureFeatureEndpoints : IInfrastructureRootAccess
{
    string Feature(string name)
    {
        return $"{((IHomaRootAccess)this).Root}/client/features/{name}";
    }
}