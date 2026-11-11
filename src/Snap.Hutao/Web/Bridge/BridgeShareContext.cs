// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Web.WebView2.Core;
using Snap.Hutao.Rev.Core.DataTransfer;
using Snap.Hutao.Rev.Factory.Picker;
using System.Net.Http;

namespace Snap.Hutao.Rev.Web.Bridge;

internal sealed class BridgeShareContext
{
    public required CoreWebView2 CoreWebView2 { get; init; }

    public required ITaskContext TaskContext { get; init; }

    public required HttpClient HttpClient { get; init; }

    public required IClipboardProvider ClipboardProvider { get; init; }

    public required JsonSerializerOptions JsonSerializerOptions { get; init; }

    public required IFileSystemPickerInteraction FileSystemPickerInteraction { get; init; }

    public required BridgeShareSaveType ShareSaveType { get; init; }

    public required IMessenger Messenger { get; init; }
}