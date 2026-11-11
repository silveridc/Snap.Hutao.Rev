// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureGitRepositoryEndpoints : IInfrastructureRootAccess
{
    string GitRepository(string name)
    {
        return $"{((IHomaRootAccess)this).UnprefixedRoot}/git-repository/all?name={name}";
    }
}