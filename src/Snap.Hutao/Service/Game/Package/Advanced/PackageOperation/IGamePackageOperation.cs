// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Game.Package.Advanced.PackageOperation;

internal interface IGamePackageOperation
{
    ValueTask ExecuteAsync(GamePackageServiceContext context);
}