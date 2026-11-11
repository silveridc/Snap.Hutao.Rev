// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Property;
using Snap.Hutao.Rev.Service.Game.PathAbstraction;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Game.FileSystem;

internal interface IRestrictedGamePathAccess
{
    IObservableProperty<GamePathEntry?> GamePathEntry { get; }

    IObservableProperty<ImmutableArray<GamePathEntry>> GamePathEntries { get; }

    AsyncReaderWriterLock GamePathLock { get; }
}