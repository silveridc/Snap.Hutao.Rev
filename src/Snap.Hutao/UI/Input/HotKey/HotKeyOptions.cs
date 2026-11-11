// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Model;
using Snap.Hutao.Rev.Service.Game;
using Snap.Hutao.Rev.Win32;
using Snap.Hutao.Rev.Win32.UI.Input.KeyboardAndMouse;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.UI.Input.HotKey;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class HotKeyOptions : ObservableObject, IDisposable
{
    private readonly IServiceProvider serviceProvider;
    private readonly ITaskContext taskContext;

    private bool isDisposed;

    [GeneratedConstructor]
    public partial HotKeyOptions(IServiceProvider serviceProvider);

    static unsafe HotKeyOptions()
    {
        HotKeyAction.BeforeSwitch = static () => IsInGameOnly && !GameLifeCycle.IsGameRunningRequiresMainThread();
    }

    public static bool IsInGameOnly
    {
        get => LocalSetting.Get(SettingKeys.HotKeyRepeatForeverInGameOnly, false);
        set => LocalSetting.Set(SettingKeys.HotKeyRepeatForeverInGameOnly, value);
    }

    public static ImmutableArray<NameValue<VIRTUAL_KEY>> VirtualKeys { get => Input.VirtualKeys.HotKeyValues; }

    [ObservableProperty]
    public partial HotKeyCombination? MouseClickRepeatForeverKeyCombination { get; set; }

    [ObservableProperty]
    public partial HotKeyCombination? KeyPressRepeatForeverKeyCombination { get; set; }

    public async ValueTask InitializeAsync()
    {
        await taskContext.SwitchToMainThreadAsync();

        MouseClickRepeatForeverKeyCombination = new(serviceProvider, HutaoNativeHotKeyActionKind.MouseClickRepeatForever, SettingKeys.HotKeyMouseClickRepeatForever);
        KeyPressRepeatForeverKeyCombination = new(serviceProvider, HutaoNativeHotKeyActionKind.KeyPressRepeatForever, SettingKeys.HotKeyKeyPressRepeatForever);

        MouseClickRepeatForeverKeyCombination.Initialize();
        KeyPressRepeatForeverKeyCombination.Initialize();
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref isDisposed, true))
        {
            return;
        }

        MouseClickRepeatForeverKeyCombination?.Dispose();
        KeyPressRepeatForeverKeyCombination?.Dispose();
    }
}