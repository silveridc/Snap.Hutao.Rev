// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Diagnostics;

[SuppressMessage("", "SH001", Justification = "IHutaoDiagnostics must be public in order to be exposed to the scripting environment")]
public interface IHutaoDiagnostics
{
    IReadOnlyDictionary<string, object?> LocalSettings { get; }

    ValueTask<int> ExecuteSqlAsync(string sql);

    Dictionary<string, string> MakeApplicationDataCompositeValue(params string[] keys);
}
