// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.CompilerServices;

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureEndpoints :
    IInfrastructureEnkaEndpoints,
    IInfrastructureStrategyEndpoints,
    IInfrastructureFeatureEndpoints,
    IInfrastructureMetadataEndpoints,
    IInfrastructurePatchEndpoints,
    IInfrastructureGitRepositoryEndpoints,
    IInfrastructureWallpaperEndpoints,
    IInfrastructureRootAccess,
    IInfrastructureManagementEndpoints
{
    string Ip() => $"{Unsafe.As<IHomaRootAccess>(this).Root}/public/ip";
}