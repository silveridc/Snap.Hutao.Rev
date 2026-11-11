// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Hutao.Rev.Service.Hutao;
using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.Web.Hutao;
using Snap.Hutao.Rev.Web.Hutao.Response;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Snap.Hutao.Rev.ViewModel.HutaoPassport;

[Service(ServiceLifetime.Scoped)]
internal sealed partial class HutaoPassportLoginLogViewModel : Abstraction.ViewModel
{
    private readonly HutaoUserOptions hutaoUserOptions;
    private readonly HutaoPassportClient hutaoPassportClient;
    private readonly ITaskContext taskContext;
    private readonly IMessenger messenger;

    [GeneratedConstructor]
    public partial HutaoPassportLoginLogViewModel(IServiceProvider serviceProvider);

    [ObservableProperty]
    public partial ObservableCollection<AccountLoginLogView> Logs { get; set; } = [];

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    protected override async ValueTask<bool> LoadOverrideAsync(CancellationToken token)
    {
        string? accessToken = await hutaoUserOptions.GetAccessTokenAsync(token).ConfigureAwait(false);
        if (string.IsNullOrEmpty(accessToken))
        {
            messenger.Send(InfoBarMessage.Error(SH.ViewModelHutaoPassportLoginLogRequiresLogin));
            return false;
        }

        HutaoResponse<AccountLoginLogPage> response = await hutaoPassportClient
            .GetLoginLogsAsync(accessToken, 1, 50, token)
            .ConfigureAwait(false);

        if (response.ReturnCode is not 0)
        {
            messenger.Send(InfoBarMessage.Error(SH.ViewModelHutaoPassportLoginLogLoadFailed));
            return false;
        }

        List<AccountLoginLogView> rows = [.. (response.Data?.Items ?? []).Select(AccountLoginLogView.From)];

        await taskContext.SwitchToMainThreadAsync();
        Logs = new(rows);
        IsEmpty = rows.Count is 0;
        return true;
    }
}

internal sealed class AccountLoginLogView
{
    public required string Time { get; init; }

    public required string Method { get; init; }

    public required string Result { get; init; }

    public required string Ip { get; init; }

    public required string Region { get; init; }

    public required string Device { get; init; }

    public required bool Success { get; init; }

    public static AccountLoginLogView From(AccountLoginLogEntry entry)
    {
        return new()
        {
            Time = entry.LoginAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture),
            Method = entry.Method is "sso" ? SH.ViewModelHutaoPassportLoginLogMethodSso : SH.ViewModelHutaoPassportLoginLogMethodPassword,
            Result = entry.Success ? SH.ViewModelHutaoPassportLoginLogResultSuccess : SH.ViewModelHutaoPassportLoginLogResultFailure,
            Ip = string.IsNullOrEmpty(entry.Ip) ? "-" : entry.Ip,
            Region = FormatRegion(entry.IpRegion),
            Device = FormatDevice(entry),
            Success = entry.Success,
        };
    }

    private static string FormatRegion(string?[]? region)
    {
        if (region is not { Length: > 0 })
        {
            return "-";
        }

        // IP2Region 的缺段是 "0"，去重后拼成可读文本
        string[] parts = [.. region.OfType<string>().Select(part => part.Trim()).Where(part => part.Length > 0 && part is not "0").Distinct()];
        return parts.Length is 0 ? "-" : string.Join(' ', parts);
    }

    private static string FormatDevice(AccountLoginLogEntry entry)
    {
        string name = entry.DeviceName ?? entry.DeviceId ?? string.Empty;
        if (string.IsNullOrEmpty(name))
        {
            return "-";
        }

        return string.IsNullOrEmpty(entry.OperatingSystem) ? name : $"{name} · {entry.OperatingSystem}";
    }
}
