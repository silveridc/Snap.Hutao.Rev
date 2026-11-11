#include <Windows.h>
#include <TlHelp32.h>

#include <string>

namespace
{
    constexpr DWORD kLoadLibraryTimeoutMs = 30'000;
    constexpr DWORD kRemoteModuleTimeoutMs = 5'000;

    struct UniqueHandle
    {
        explicit UniqueHandle(HANDLE handle = nullptr) noexcept
            : Value(handle)
        {
        }

        UniqueHandle(const UniqueHandle&) = delete;
        UniqueHandle& operator=(const UniqueHandle&) = delete;

        ~UniqueHandle()
        {
            if (Value != nullptr && Value != INVALID_HANDLE_VALUE)
            {
                CloseHandle(Value);
            }
        }

        explicit operator bool() const noexcept
        {
            return Value != nullptr && Value != INVALID_HANDLE_VALUE;
        }

        HANDLE Value;
    };

    struct RemoteBuffer
    {
        RemoteBuffer(HANDLE process, void* address) noexcept
            : Process(process), Address(address)
        {
        }

        RemoteBuffer(const RemoteBuffer&) = delete;
        RemoteBuffer& operator=(const RemoteBuffer&) = delete;

        ~RemoteBuffer()
        {
            VirtualFreeEx(Process, Address, 0, MEM_RELEASE);
        }

        HANDLE Process;
        void* Address;
    };

    struct ScopedModule
    {
        explicit ScopedModule(HMODULE module) noexcept
            : Value(module)
        {
        }

        ScopedModule(const ScopedModule&) = delete;
        ScopedModule& operator=(const ScopedModule&) = delete;

        ~ScopedModule()
        {
            FreeLibrary(Value);
        }

        HMODULE Value;
    };

    [[nodiscard]] HRESULT HResultFromLastError() noexcept
    {
        return HRESULT_FROM_WIN32(GetLastError());
    }

    [[nodiscard]] std::string Narrow(PCWSTR value)
    {
        int length = WideCharToMultiByte(CP_ACP, 0, value, -1, nullptr, 0, nullptr, nullptr);
        if (length <= 1)
        {
            return {};
        }

        std::string result(static_cast<size_t>(length - 1), '\0');
        WideCharToMultiByte(CP_ACP, 0, value, -1, result.data(), length, nullptr, nullptr);
        return result;
    }

    [[nodiscard]] std::wstring FileNameOf(PCWSTR path)
    {
        std::wstring fileName(path);
        size_t separator = fileName.find_last_of(L"\\/");
        if (separator != std::wstring::npos)
        {
            fileName.erase(0, separator + 1);
        }

        return fileName;
    }

    // 远线程 LoadLibraryW 并把 DLL 路径写进目标进程；
    // 与原版不同的是这里会检查线程退出码（HMODULE），加载失败不会再被当成成功。
    [[nodiscard]] HRESULT InjectLoadLibrary(HANDLE process, PCWSTR dllPath)
    {
        SIZE_T pathSize = (wcslen(dllPath) + 1) * sizeof(wchar_t);

        void* remotePath = VirtualAllocEx(process, nullptr, pathSize, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
        if (remotePath == nullptr)
        {
            return HResultFromLastError();
        }

        RemoteBuffer buffer(process, remotePath);

        if (WriteProcessMemory(process, remotePath, dllPath, pathSize, nullptr) == FALSE)
        {
            return HResultFromLastError();
        }

        HMODULE kernel32 = GetModuleHandleW(L"kernel32.dll");
        if (kernel32 == nullptr)
        {
            return HResultFromLastError();
        }

        auto loadLibrary = reinterpret_cast<LPTHREAD_START_ROUTINE>(GetProcAddress(kernel32, "LoadLibraryW"));
        if (loadLibrary == nullptr)
        {
            return HResultFromLastError();
        }

        UniqueHandle thread(CreateRemoteThread(process, nullptr, 0, loadLibrary, remotePath, 0, nullptr));
        if (!thread)
        {
            return HResultFromLastError();
        }

        if (WaitForSingleObject(thread.Value, kLoadLibraryTimeoutMs) != WAIT_OBJECT_0)
        {
            return HRESULT_FROM_WIN32(ERROR_TIMEOUT);
        }

        DWORD exitCode = 0;
        if (GetExitCodeThread(thread.Value, &exitCode) == FALSE)
        {
            return HResultFromLastError();
        }

        return exitCode != 0 ? S_OK : HRESULT_FROM_WIN32(ERROR_MOD_NOT_FOUND);
    }

    [[nodiscard]] HRESULT FindRemoteModule(DWORD processId, PCWSTR moduleName, BYTE*& baseAddress)
    {
        ULONGLONG start = GetTickCount64();
        for (;;)
        {
            UniqueHandle snapshot(CreateToolhelp32Snapshot(TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32, processId));
            if (snapshot)
            {
                MODULEENTRY32W entry{};
                entry.dwSize = sizeof(entry);
                if (Module32FirstW(snapshot.Value, &entry) != FALSE)
                {
                    do
                    {
                        if (_wcsicmp(entry.szModule, moduleName) == 0)
                        {
                            baseAddress = entry.modBaseAddr;
                            return S_OK;
                        }
                    } while (Module32NextW(snapshot.Value, &entry) != FALSE);
                }
            }

            if (GetTickCount64() - start > kRemoteModuleTimeoutMs)
            {
                return HRESULT_FROM_WIN32(ERROR_TIMEOUT);
            }

            Sleep(50);
        }
    }
} // namespace

extern "C"
{
    __declspec(dllexport) HRESULT __stdcall HutaoInjectUsingRemoteThread(PCWSTR dllPath, int processId)
    {
        if (dllPath == nullptr)
        {
            return E_POINTER;
        }

        if (processId == 0)
        {
            return E_INVALIDARG;
        }

        UniqueHandle process(OpenProcess(PROCESS_ALL_ACCESS, FALSE, static_cast<DWORD>(processId)));
        if (!process)
        {
            return HResultFromLastError();
        }

        return InjectLoadLibrary(process.Value, dllPath);
    }

    __declspec(dllexport) HRESULT __stdcall HutaoInjectUsingRemoteThreadWithFunction(PCWSTR dllPath, PCWSTR functionName, int processId)
    {
        if (dllPath == nullptr || functionName == nullptr)
        {
            return E_POINTER;
        }

        if (processId == 0)
        {
            return E_INVALIDARG;
        }

        HRESULT hr = HutaoInjectUsingRemoteThread(dllPath, processId);
        if (FAILED(hr))
        {
            return hr;
        }

        std::wstring moduleName = FileNameOf(dllPath);
        BYTE* remoteBase = nullptr;
        hr = FindRemoteModule(static_cast<DWORD>(processId), moduleName.c_str(), remoteBase);
        if (FAILED(hr))
        {
            return hr;
        }

        HMODULE localModule = LoadLibraryExW(dllPath, nullptr, LOAD_WITH_ALTERED_SEARCH_PATH);
        if (localModule == nullptr)
        {
            return HResultFromLastError();
        }

        ScopedModule scopedModule(localModule);

        std::string procedureName = Narrow(functionName);
        FARPROC localFunction = GetProcAddress(localModule, procedureName.c_str());
        if (localFunction == nullptr)
        {
            return HResultFromLastError();
        }

        BYTE* remoteFunction = remoteBase + (reinterpret_cast<BYTE*>(localFunction) - reinterpret_cast<BYTE*>(localModule));

        UniqueHandle process(OpenProcess(PROCESS_ALL_ACCESS, FALSE, static_cast<DWORD>(processId)));
        if (!process)
        {
            return HResultFromLastError();
        }

        UniqueHandle thread(CreateRemoteThread(process.Value, nullptr, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(remoteFunction), nullptr, 0, nullptr));
        if (!thread)
        {
            return HResultFromLastError();
        }

        return S_OK;
    }
}
