// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Windows.AppNotifications;

namespace Snap.Hutao.Rev.Service.Notification;

[Service(ServiceLifetime.Singleton, typeof(IAppNotificationLifeTime))]
internal sealed partial class AppNotificationLifeTime : IAppNotificationLifeTime
{
    public void Dispose()
    {
        // 用于在程序退出时尝试清除所有的系统通知
        try
        {
            // 退出路径：最多等 2 秒，避免通知服务无响应时卡住退出
            AppNotificationManager.Default.RemoveAllAsync().AsTask().Wait(TimeSpan.FromSeconds(2));
            AppNotificationManager.Default.Unregister();
        }
        catch
        {
            // Ignored
        }
    }
}