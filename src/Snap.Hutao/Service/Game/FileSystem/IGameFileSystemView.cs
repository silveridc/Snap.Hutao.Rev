// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Game.FileSystem;

internal interface IGameFileSystemView
{
    bool IsDisposed { get; }

    string GameFilePath { get; }

    GameAudioInstallation Audio { get; }
}