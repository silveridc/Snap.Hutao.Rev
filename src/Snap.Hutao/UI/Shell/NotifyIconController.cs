// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.LifeCycle;
using Snap.Hutao.Rev.Factory.ContentDialog;
using Snap.Hutao.Rev.UI.Windowing;
using Snap.Hutao.Rev.UI.Xaml.View.Window;
using Snap.Hutao.Rev.Win32;
using Snap.Hutao.Rev.Win32.Foundation;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Rev.UI.Shell;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class NotifyIconController : IDisposable
{
    private static bool constructed;

    private readonly Lock syncRoot = new();

    private readonly ICurrentXamlWindowReference currentXamlWindowReference;
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly LazySlim<NotifyIconContextMenu> lazyMenu;
    private readonly NotifyIconXamlHostWindow xamlHostWindow;
    private readonly IServiceProvider serviceProvider;
    private readonly HutaoNotifyIcon native;

    private bool disposed;

    public NotifyIconController(IServiceProvider serviceProvider)
    {
        if (Interlocked.Exchange(ref constructed, true))
        {
            // Actively prevent multiple constructions, if this happens, it's definitely a bug.
            // For example: the below part of the ctor throws an exception.
            throw new InvalidOperationException("NotifyIconController is already constructed.");
        }

        currentXamlWindowReference = serviceProvider.GetRequiredService<ICurrentXamlWindowReference>();
        contentDialogFactory = serviceProvider.GetRequiredService<IContentDialogFactory>();
        this.serviceProvider = serviceProvider;
        lazyMenu = new(() => new(serviceProvider));

        string iconPath = InstalledLocation.GetAbsolutePath("Assets/Logo.ico");
        native = new(iconPath);

        xamlHostWindow = new(serviceProvider);
        xamlHostWindow.MoveAndResize(default);
    }

    public static Lock InitializationSyncRoot { get; } = new();

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        lock (syncRoot)
        {
            disposed = true;
            try
            {
                native.Destroy();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }

    public void Create()
    {
        native.Create(OnNotifyIconCallback, "Snap Hutao");
    }

    public bool IsPromoted()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        try
        {
            return native.IsPromoted;
        }
        catch (Exception ex)
        {
            // If the lpValue registry value does not exist, the function returns ERROR_FILE_NOT_FOUND
            if (ex is not (FileNotFoundException or COMException or ObjectDisposedException))
            {
                SentrySdk.CaptureException(ex);
            }

            return false;
        }
    }

    private void OnNotifyIconCallback(HutaoNativeNotifyIconCallbackKind kind, RECT icon, POINT point)
    {
        switch (kind)
        {
            case HutaoNativeNotifyIconCallbackKind.TaskbarCreated:
                OnRecreateNotifyIconRequested();
                break;
            case HutaoNativeNotifyIconCallbackKind.ContextMenu:
            case HutaoNativeNotifyIconCallbackKind.LeftButtonDown:
                OnContextMenuRequested(icon, point);
                break;
            case HutaoNativeNotifyIconCallbackKind.LeftButtonDoubleClick:
                OnWindowRequested();
                break;
        }
    }

    private void OnRecreateNotifyIconRequested()
    {
        if (disposed || XamlApplicationLifetime.Exiting)
        {
            return;
        }

        native.Recreate("Snap Hutao");
    }

    private void OnContextMenuRequested(RECT icon, POINT point)
    {
        if (disposed)
        {
            return;
        }

        if (XamlApplicationLifetime.Exiting)
        {
            Debugger.Break();
            return;
        }

        // https://github.com/DGP-Studio/Snap.Hutao/issues/2434
        // Now we disable the context menu when the dialog is showing.
        if (contentDialogFactory.IsDialogShowing)
        {
            return;
        }

        xamlHostWindow.ShowFlyoutAt(lazyMenu.Value, new(point.x, point.y), icon);
    }

    private void OnWindowRequested()
    {
        if (disposed)
        {
            return;
        }

        if (XamlApplicationLifetime.Exiting)
        {
            Debugger.Break();
            return;
        }

        switch (currentXamlWindowReference.Window)
        {
            case null:
                {
                    // MainWindow is closed, show it
                    MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                    currentXamlWindowReference.Window = mainWindow;
                    mainWindow.SwitchTo();
                    mainWindow.AppWindow.MoveInZOrderAtTop();
                    return;
                }

            default:
                {
                    Window window = currentXamlWindowReference.Window;

                    // While window is closing, currentXamlWindowReference can still retrieve the window,
                    // just ignore it
                    if (window.AppWindow is not null)
                    {
                        window.SwitchTo();
                        window.AppWindow.MoveInZOrderAtTop();
                    }

                    return;
                }
        }
    }
}