// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Game.Package.Advanced.Model;

namespace Snap.Hutao.Rev.Service.Game.Package.Advanced.PackageOperation;

[Service(ServiceLifetime.Transient, typeof(IGamePackageOperation), Key = GamePackageOperationKind.Verify)]
internal sealed class GamePackageVerifyOperation : GamePackageOperation
{
    public override async ValueTask ExecuteAsync(GamePackageServiceContext context)
    {
        SophonDecodedBuild? localBuild = context.Operation.LocalBuild;
        ArgumentNullException.ThrowIfNull(localBuild);
        await PrivateVerifyAndRepairAsync(context, localBuild, localBuild.UncompressedTotalBytes, localBuild.TotalChunks).ConfigureAwait(false);
    }
}