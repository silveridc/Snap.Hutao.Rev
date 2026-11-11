// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.Foundation;

namespace Snap.Hutao.Rev.Core.Diagnostics;

internal interface IProcess : IDisposable
{
    int Id { get; }

    nint Handle { get; }

    HWND MainWindowHandle { get; }

    bool HasExited { get; }

    int ExitCode { get; }

    void Start();

    void ResumeMainThread();

    void WaitForExit();

    void Kill();
}