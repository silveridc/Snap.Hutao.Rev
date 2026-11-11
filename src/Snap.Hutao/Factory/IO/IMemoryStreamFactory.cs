// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.IO;

namespace Snap.Hutao.Rev.Factory.IO;

internal interface IMemoryStreamFactory
{
    MemoryStream GetStream();

    MemoryStream GetStreamExactly(long requiredSize);
}