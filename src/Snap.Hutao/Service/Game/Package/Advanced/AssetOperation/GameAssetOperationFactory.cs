// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Service.Game.Package.Advanced.AssetOperation;

[Service(ServiceLifetime.Transient, typeof(IDriverMediaTypeAwareFactory<IGameAssetOperation>))]
internal sealed partial class GameAssetOperationFactory : DriverMediaTypeAwareFactory<IGameAssetOperation, GameAssetOperationSSD, GameAssetOperationHDD>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial GameAssetOperationFactory(IServiceProvider serviceProvider);
}