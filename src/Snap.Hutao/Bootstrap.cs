// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.Rev.Core;
using Snap.Hutao.Rev.Core.Logging;
using Snap.Hutao.Rev.Core.Security.Principal;
using Snap.Hutao.Rev.Win32;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using WinRT;

[assembly: DisableRuntimeMarshalling]

namespace Snap.Hutao.Rev;

[SuppressMessage("", "SH001")]
public static partial class Bootstrap
{
    private const string LockName = "SNAP_HUTAO_BOOTSTRAP_LOCK";
    private static readonly ApplicationInitializationCallback AppInitializationCallback = InitializeApp;
    private static Mutex? mutex;

    internal static void UseNamedPipeRedirection()
    {
        Debug.Assert(mutex is not null);
        DisposableMarshal.DisposeAndClear(ref mutex);
    }

    [STAThread]
    private static void Main(string[] args)
    {
        if (Mutex.TryOpenExisting(LockName, out _))
        {
            return;
        }

        try
        {
            MutexSecurity mutexSecurity = new();
            mutexSecurity.AddAccessRule(new(SecurityIdentifiers.Everyone, MutexRights.FullControl, AccessControlType.Allow));
            mutex = MutexAcl.Create(true, LockName, out bool created, mutexSecurity);
            Debug.Assert(created);
        }
        catch (WaitHandleCannotBeOpenedException)
        {
            return;
        }

        using (mutex)
        {
            if (!OSPlatformSupported())
            {
                return;
            }

            Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "00000000");
            Environment.SetEnvironmentVariable("DOTNET_SYSTEM_BUFFERS_SHAREDARRAYPOOL_MAXARRAYSPERPARTITION", "128");
            AppContext.SetData("MVVMTOOLKIT_ENABLE_INOTIFYPROPERTYCHANGING_SUPPORT", false);

            ComWrappersSupport.InitializeComWrappers();
            using (ServiceProvider serviceProvider = DependencyInjection.Initialize())
            {
                Thread.CurrentThread.Name = "Snap Hutao Rev Application Main Thread";
                Application.Start(AppInitializationCallback);
                XamlApplicationLifetime.Exited = true;
            }

            SentrySdk.Flush();
        }
    }

    private static void InitializeApp(ApplicationInitializationCallbackParams param)
    {
        Gen2GcCallback.Register(() =>
        {
            SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateDebug("Gen2 GC triggered.", "Runtime"));
            return true;
        });

        IServiceProvider serviceProvider = Ioc.Default;

        _ = serviceProvider.GetRequiredService<ITaskContext>();
        _ = serviceProvider.GetRequiredService<App>();
    }

    private static bool OSPlatformSupported()
    {
        if (!NativeSystem.IsCurrentWindowsVersionSupported())
        {
            const string Message = """
                Snap Hutao 无法在版本低于 10.0.19045.5371 的 Windows 上运行，请更新系统。
                Snap Hutao cannot run on Windows versions earlier than 10.0.19045.5371. Please update your system.
                """;
            NativeSystem.ShowErrorMessage("Warning | 警告", Message);
            return false;
        }

        return true;
    }
}