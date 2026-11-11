// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Win32;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace Snap.Hutao.Rev.Core.IO.Http.Proxy;

[SuppressMessage("", "CA1001")]
internal sealed partial class HttpProxyUsingSystemProxy : ObservableObject, IWebProxy
{
    private const string ProxySettingPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Connections";

    private static readonly Uri ProxyTestDestination = ServerHosts.Current.Infrastructure.ToUri();

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly RegistryNotification native;

    private unsafe HttpProxyUsingSystemProxy()
    {
        InnerProxy = ConstructSystemProxy(null);

        native = new(ProxySettingPath);
        native.Start(OnSystemProxySettingsChanged);
    }

    [field: MaybeNull]
    public static HttpProxyUsingSystemProxy Instance { get => LazyInitializer.EnsureInitialized(ref field, () => new()); }

    public string DisplayProxyUri { get => CurrentProxyUri ?? SH.ViewPageFeedbackCurrentProxyNoProxyDescription; }

    public string? CurrentProxyUri { get => GetProxy(ProxyTestDestination)?.AbsoluteUri; }

    public IWebProxy InnerProxy
    {
        get;
        set
        {
            if (ReferenceEquals(field, value))
            {
                return;
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            (field as IDisposable)?.Dispose();
            field = value;
        }
    }

    public ICredentials? Credentials
    {
        get => InnerProxy.Credentials;
        set => InnerProxy.Credentials = value;
    }

    public Uri? GetProxy(Uri destination)
    {
        return InnerProxy.GetProxy(destination);
    }

    public bool IsBypassed(Uri host)
    {
        return InnerProxy.IsBypassed(host);
    }

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "ConstructSystemProxy")]
    private static extern IWebProxy ConstructSystemProxy([UnsafeAccessorType("System.Net.Http.SystemProxyInfo, System.Net.Http")] object? c);

    private static void OnSystemProxySettingsChanged()
    {
        if (XamlApplicationLifetime.Exiting)
        {
            return;
        }

        Instance.InnerProxy = ConstructSystemProxy(null);

        Debug.Assert(XamlApplicationLifetime.DispatcherQueueInitialized, "DispatcherQueue not initialized");
        SynchronizationContext.Current?.Post(static _ => Instance.OnPropertyChanged(nameof(DisplayProxyUri)), default);
    }
}