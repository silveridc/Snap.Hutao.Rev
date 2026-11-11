// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;
using Snap.Hutao.Rev.Service.Game.FileSystem;
using Snap.Hutao.Rev.Service.Game.Package.Advanced.AssetOperation;
using Snap.Hutao.Rev.Service.Game.Package.Advanced.Model;
using Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.ChannelSDK;
using System.IO;

namespace Snap.Hutao.Rev.Service.Game.Package.Advanced;

internal sealed class GamePackageOperationContext
{
    public GamePackageOperationContext(IServiceProvider serviceProvider, GamePackageOperationKind kind, IGameFileSystem gameFileSystem, string? extractDirectory = default)
    {
        Kind = kind;
        Asset = serviceProvider.GetRequiredService<IDriverMediaTypeAwareFactory<IGameAssetOperation>>().Create(string.Empty);
        GameFileSystem = gameFileSystem;

        EffectiveGameDirectory = extractDirectory ?? gameFileSystem.GameDirectory;

        EffectiveChunksDirectory = kind is GamePackageOperationKind.Verify
            ? Path.Combine(gameFileSystem.ChunksDirectory, "repair")
            : gameFileSystem.ChunksDirectory;
    }

    public GamePackageOperationKind Kind { get; }

    public IGameAssetOperation Asset { get; }

    public IGameFileSystem GameFileSystem { get; init; }

    public SophonDecodedBuild? LocalBuild { get; init; }

    public SophonDecodedBuild? RemoteBuild { get; init; }

    public SophonDecodedPatchBuild? PatchBuild { get; init; }

    public GameChannelSDK? GameChannelSDK { get; init; }

    public string EffectiveGameDirectory { get; }

    public string EffectiveChunksDirectory { get; }
}