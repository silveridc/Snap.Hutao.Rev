// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

#include <Windows.h>
#include <Shellapi.h>

#include <cstdio>
#include <string>
#include <string_view>

namespace
{
    constexpr int kExitSuccess = 0;
    constexpr int kExitShellExecuteFailed = 1;
    constexpr int kExitUsage = 2;

    // Package family names consist of the package name and the publisher id, both are
    // restricted to alphanumerics, dots, dashes and underscores.
    bool IsValidPackageFamilyName(std::wstring_view familyName) noexcept
    {
        if (familyName.empty() || familyName.size() > 128)
        {
            return false;
        }

        for (wchar_t character : familyName)
        {
            const bool isDigit = character >= L'0' && character <= L'9';
            const bool isUpper = character >= L'A' && character <= L'Z';
            const bool isLower = character >= L'a' && character <= L'z';
            const bool isSeparator = character == L'.' || character == L'-' || character == L'_';
            if (!isDigit && !isUpper && !isLower && !isSeparator)
            {
                return false;
            }
        }

        return true;
    }

    void ShowError(PCWSTR message) noexcept
    {
        MessageBoxW(nullptr, message, L"Snap.Hutao", MB_OK | MB_ICONERROR);
    }

    void ShowShellExecuteError(DWORD error) noexcept
    {
        wchar_t message[256];
        swprintf_s(message, L"以管理员身份启动胡桃失败。\n\n错误代码: %lu", error);
        ShowError(message);
    }
}

int WINAPI wWinMain(_In_ HINSTANCE instance, _In_opt_ HINSTANCE previousInstance, _In_ PWSTR commandLine, _In_ int showCommand)
{
    UNREFERENCED_PARAMETER(instance);
    UNREFERENCED_PARAMETER(previousInstance);
    UNREFERENCED_PARAMETER(commandLine);
    UNREFERENCED_PARAMETER(showCommand);

    int argumentCount = 0;
    LPWSTR* arguments = CommandLineToArgvW(GetCommandLineW(), &argumentCount);
    if (arguments == nullptr)
    {
        ShowError(L"无法解析命令行参数。");
        return kExitUsage;
    }

    std::wstring appUserModelId;
    if (argumentCount >= 2 && IsValidPackageFamilyName(arguments[1]))
    {
        appUserModelId = std::wstring(L"shell:AppsFolder\\").append(arguments[1]).append(L"!App");
    }

    LocalFree(arguments);

    if (appUserModelId.empty())
    {
        ShowError(L"用法: Snap.Hutao.Elevated.Launcher.exe <PackageFamilyName>");
        return kExitUsage;
    }

    SHELLEXECUTEINFOW info{};
    info.cbSize = sizeof(info);
    // SEE_MASK_NOASYNC keeps the strings alive until the UAC prompt is answered.
    info.fMask = SEE_MASK_NOCLOSEPROCESS | SEE_MASK_NOASYNC;
    info.lpVerb = L"runas";
    info.lpFile = appUserModelId.c_str();
    info.nShow = SW_SHOWNORMAL;

    if (!ShellExecuteExW(&info))
    {
        const DWORD error = GetLastError();
        if (error == ERROR_CANCELLED)
        {
            // The user declined the UAC prompt, that is not an error worth a message box.
            return kExitSuccess;
        }

        ShowShellExecuteError(error);
        return kExitShellExecuteFailed;
    }

    if (info.hProcess != nullptr)
    {
        CloseHandle(info.hProcess);
    }

    return kExitSuccess;
}
