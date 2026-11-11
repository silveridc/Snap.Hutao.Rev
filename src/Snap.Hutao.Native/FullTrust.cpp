#include <Windows.h>

#include <cstdint>
#include <cstdio>
#include <cstdlib>
#include <memory>
#include <stdexcept>
#include <string>
#include <vector>

#include "XxHash64.h"

namespace
{
    constexpr wchar_t kPipeName[] = L"\\\\.\\pipe\\Snap.Hutao.PrivateFullTrustNamedPipe.FpsV3";
    constexpr uint8_t kProtocolVersion = 3;

    constexpr uint32_t kCreateSuspended = 0x00000004;
    constexpr uint32_t kStillActive = 259;
    constexpr int kWindowRetryCount = 300;
    constexpr DWORD kWindowRetryDelayMs = 200;
    constexpr DWORD kLoadLibraryTimeoutMs = 60'000;

    enum class PacketType : uint8_t
    {
        None = 0,
        Request = 1,
        Response = 2,
        SessionTermination = 3,
    };

    enum class PacketCommand : uint8_t
    {
        None = 0,
        Create = 1,
        StartProcess = 2,
        LoadLibrary = 3,
        ResumeMainThread = 4,
    };

    enum class ContentType : uint8_t
    {
        None = 0,
        Json = 1,
    };

#pragma pack(push, 1)
    struct PacketHeader
    {
        uint8_t Version;
        PacketType Type;
        PacketCommand Command;
        ContentType Content;
        int32_t ContentLength;
        uint64_t Checksum;
    };
#pragma pack(pop)

    static_assert(sizeof(PacketHeader) == 16, "报文头必须与 C# 侧一致");

    // 管道或报文异常按 IOException 处理：直接结束会话，不再回复。
    struct PipeException
    {
    };

    struct ProtocolError : std::runtime_error
    {
        using std::runtime_error::runtime_error;
    };

    [[nodiscard]] std::string LastErrorText(const char* operation)
    {
        return std::string(operation) + " failed with Win32 error " + std::to_string(GetLastError());
    }

    [[nodiscard]] std::wstring Utf8ToWide(const std::string& text)
    {
        if (text.empty())
        {
            return {};
        }

        int length = MultiByteToWideChar(CP_UTF8, 0, text.data(), static_cast<int>(text.size()), nullptr, 0);
        if (length <= 0)
        {
            return {};
        }

        std::wstring result(length, L'\0');
        MultiByteToWideChar(CP_UTF8, 0, text.data(), static_cast<int>(text.size()), result.data(), length);
        return result;
    }

    [[nodiscard]] bool ReadExactly(HANDLE pipe, void* buffer, DWORD length)
    {
        auto* p = static_cast<uint8_t*>(buffer);
        DWORD remaining = length;
        while (remaining > 0)
        {
            DWORD read = 0;
            if (ReadFile(pipe, p, remaining, &read, nullptr) == FALSE || read == 0)
            {
                return false;
            }

            p += read;
            remaining -= read;
        }

        return true;
    }

    void WriteAll(HANDLE pipe, const void* buffer, DWORD length)
    {
        auto* p = static_cast<const uint8_t*>(buffer);
        DWORD remaining = length;
        while (remaining > 0)
        {
            DWORD written = 0;
            if (WriteFile(pipe, p, remaining, &written, nullptr) == FALSE || written == 0)
            {
                throw PipeException();
            }

            p += written;
            remaining -= written;
        }
    }

    void WritePacket(HANDLE pipe, PacketType type, PacketCommand command)
    {
        PacketHeader header{ kProtocolVersion, type, command, ContentType::None, 0, 0 };
        WriteAll(pipe, &header, sizeof(header));
    }

    void WritePacket(HANDLE pipe, PacketType type, PacketCommand command, const std::string& content)
    {
        PacketHeader header{ kProtocolVersion, type, command, ContentType::Json, static_cast<int32_t>(content.size()), 0 };
        header.Checksum = XxHash64::Hash(content.data(), content.size());
        WriteAll(pipe, &header, sizeof(header));
        WriteAll(pipe, content.data(), static_cast<DWORD>(content.size()));
    }

    [[nodiscard]] std::vector<char> ReadContent(HANDLE pipe, const PacketHeader& header)
    {
        if (header.ContentLength < 0 || header.ContentLength > (1 << 20))
        {
            throw PipeException();
        }

        std::vector<char> content(static_cast<size_t>(header.ContentLength));
        if (!content.empty() && !ReadExactly(pipe, content.data(), static_cast<DWORD>(content.size())))
        {
            throw PipeException();
        }

        if (XxHash64::Hash(content.data(), content.size()) != header.Checksum)
        {
            throw PipeException();
        }

        return content;
    }

    // 请求只有一个扁平层级，这里只做够用的 JSON 取值，不引入完整解析器。
    [[nodiscard]] size_t FindValue(const std::string& json, const char* key)
    {
        std::string needle = std::string("\"") + key + "\"";
        size_t position = json.find(needle);
        if (position == std::string::npos)
        {
            return std::string::npos;
        }

        position = json.find(':', position + needle.size());
        if (position == std::string::npos)
        {
            return std::string::npos;
        }

        ++position;
        while (position < json.size() && (json[position] == ' ' || json[position] == '\t' || json[position] == '\r' || json[position] == '\n'))
        {
            ++position;
        }

        return position;
    }

    void AppendUtf8(std::string& target, uint32_t codePoint)
    {
        if (codePoint <= 0x7F)
        {
            target += static_cast<char>(codePoint);
        }
        else if (codePoint <= 0x7FF)
        {
            target += static_cast<char>(0xC0 | (codePoint >> 6));
            target += static_cast<char>(0x80 | (codePoint & 0x3F));
        }
        else if (codePoint <= 0xFFFF)
        {
            target += static_cast<char>(0xE0 | (codePoint >> 12));
            target += static_cast<char>(0x80 | ((codePoint >> 6) & 0x3F));
            target += static_cast<char>(0x80 | (codePoint & 0x3F));
        }
        else
        {
            target += static_cast<char>(0xF0 | (codePoint >> 18));
            target += static_cast<char>(0x80 | ((codePoint >> 12) & 0x3F));
            target += static_cast<char>(0x80 | ((codePoint >> 6) & 0x3F));
            target += static_cast<char>(0x80 | (codePoint & 0x3F));
        }
    }

    [[nodiscard]] uint32_t ReadHex4(const std::string& json, size_t position)
    {
        uint32_t value = 0;
        for (size_t i = 0; i < 4; ++i)
        {
            char c = json[position + i];
            value <<= 4;
            if (c >= '0' && c <= '9')
            {
                value |= static_cast<uint32_t>(c - '0');
            }
            else if (c >= 'a' && c <= 'f')
            {
                value |= static_cast<uint32_t>(c - 'a' + 10);
            }
            else if (c >= 'A' && c <= 'F')
            {
                value |= static_cast<uint32_t>(c - 'A' + 10);
            }
        }

        return value;
    }

    [[nodiscard]] std::string ParseStringAt(const std::string& json, size_t position)
    {
        if (position >= json.size() || json[position] != '"')
        {
            throw ProtocolError("Expected JSON string");
        }

        std::string result;
        ++position;

        while (position < json.size())
        {
            char c = json[position++];
            if (c == '"')
            {
                return result;
            }

            if (c != '\\')
            {
                result += c;
                continue;
            }

            char escape = json[position++];
            switch (escape)
            {
                case '"':
                case '\\':
                case '/':
                    result += escape;
                    break;
                case 'b':
                    result += '\b';
                    break;
                case 'f':
                    result += '\f';
                    break;
                case 'n':
                    result += '\n';
                    break;
                case 'r':
                    result += '\r';
                    break;
                case 't':
                    result += '\t';
                    break;
                case 'u':
                {
                    // 启动器用默认编码器序列化，非 ASCII 都会变成 \uXXXX。
                    uint32_t codePoint = ReadHex4(json, position);
                    position += 4;
                    if (codePoint >= 0xD800 && codePoint <= 0xDBFF && position + 6 <= json.size() && json[position] == '\\' && json[position + 1] == 'u')
                    {
                        uint32_t low = ReadHex4(json, position + 2);
                        codePoint = 0x10000 + ((codePoint - 0xD800) << 10) + (low - 0xDC00);
                        position += 6;
                    }

                    AppendUtf8(result, codePoint);
                    break;
                }
                default:
                    throw ProtocolError("Unsupported JSON escape");
            }
        }

        throw ProtocolError("Unterminated JSON string");
    }

    [[nodiscard]] std::string GetString(const std::string& json, const char* key)
    {
        size_t position = FindValue(json, key);
        return position == std::string::npos ? std::string() : ParseStringAt(json, position);
    }

    [[nodiscard]] uint32_t GetUInt32(const std::string& json, const char* key, uint32_t fallback)
    {
        size_t position = FindValue(json, key);
        if (position == std::string::npos || position >= json.size() || json[position] < '0' || json[position] > '9')
        {
            return fallback;
        }

        return static_cast<uint32_t>(strtoul(json.c_str() + position, nullptr, 10));
    }

    [[nodiscard]] std::string EscapeJson(const std::string& text)
    {
        std::string result;
        result.reserve(text.size() + 8);
        for (char c : text)
        {
            switch (c)
            {
                case '"':
                    result += "\\\"";
                    break;
                case '\\':
                    result += "\\\\";
                    break;
                case '\n':
                    result += "\\n";
                    break;
                case '\r':
                    result += "\\r";
                    break;
                case '\t':
                    result += "\\t";
                    break;
                default:
                    if (static_cast<unsigned char>(c) < 0x20)
                    {
                        char buffer[8];
                        sprintf_s(buffer, "\\u%04x", static_cast<unsigned>(c));
                        result += buffer;
                    }
                    else
                    {
                        result += c;
                    }
                    break;
            }
        }

        return result;
    }

    struct ProcessStartInfo
    {
        std::string ApplicationName;
        std::string CommandLine;
        uint32_t CreationFlags = 0;
        std::string CurrentDirectory;
    };

    struct LoadLibraryRequest
    {
        std::string LibraryPath;
    };

    class SuspendedProcess
    {
    private:
        struct UniqueHandle
        {
            explicit UniqueHandle(HANDLE value) noexcept
                : Value(value)
            {
            }

            UniqueHandle(const UniqueHandle&) = delete;
            UniqueHandle& operator=(const UniqueHandle&) = delete;

            ~UniqueHandle()
            {
                if (Value != nullptr)
                {
                    CloseHandle(Value);
                }
            }

            HANDLE Value;
        };

        struct ScopedModule
        {
            explicit ScopedModule(HMODULE value) noexcept
                : Value(value)
            {
            }

            ScopedModule(const ScopedModule&) = delete;
            ScopedModule& operator=(const ScopedModule&) = delete;

            ~ScopedModule()
            {
                if (Value != nullptr)
                {
                    FreeLibrary(Value);
                }
            }

            HMODULE Value;
        };

        HANDLE process = nullptr;
        HANDLE thread = nullptr;
        HMODULE stubModule = nullptr;
        HHOOK windowHook = nullptr;
        bool mainThreadSuspended = false;
        DWORD processId = 0;

    public:
        SuspendedProcess() = default;
        SuspendedProcess(const SuspendedProcess&) = delete;
        SuspendedProcess& operator=(const SuspendedProcess&) = delete;

        ~SuspendedProcess()
        {
            if (windowHook != nullptr)
            {
                UnhookWindowsHookEx(windowHook);
            }

            if (stubModule != nullptr)
            {
                FreeLibrary(stubModule);
            }

            if (process != nullptr)
            {
                CloseHandle(process);
            }

            if (thread != nullptr)
            {
                CloseHandle(thread);
            }
        }

        [[nodiscard]] DWORD ProcessId() const noexcept
        {
            return processId;
        }

        void Create(const ProcessStartInfo& request)
        {
            std::wstring applicationName = Utf8ToWide(request.ApplicationName);
            std::wstring commandLine = L"\"" + applicationName + L"\" " + Utf8ToWide(request.CommandLine);
            std::wstring currentDirectory = Utf8ToWide(request.CurrentDirectory);

            STARTUPINFOW startupInfo{};
            startupInfo.cb = sizeof(startupInfo);
            PROCESS_INFORMATION processInformation{};

            if (CreateProcessW(applicationName.c_str(), commandLine.data(), nullptr, nullptr, FALSE, request.CreationFlags, nullptr, currentDirectory.empty() ? nullptr : currentDirectory.c_str(), &startupInfo, &processInformation) == FALSE)
            {
                throw std::runtime_error(LastErrorText("CreateProcess"));
            }

            process = processInformation.hProcess;
            thread = processInformation.hThread;
            mainThreadSuspended = (request.CreationFlags & kCreateSuspended) != 0;
            processId = processInformation.dwProcessId;
        }

        void LoadLibraryIntoProcess(const LoadLibraryRequest& request)
        {
            if (mainThreadSuspended)
            {
                LoadLibraryUsingRemoteThread(request.LibraryPath);
                return;
            }

            LoadLibraryUsingWindowHook(request.LibraryPath);
        }

        void ResumeMainThread()
        {
            if (thread == nullptr)
            {
                return;
            }

            if (ResumeThread(thread) == static_cast<DWORD>(-1))
            {
                throw std::runtime_error(LastErrorText("ResumeThread"));
            }

            mainThreadSuspended = false;
        }

    private:
        void LoadLibraryUsingRemoteThread(const std::string& libraryPath)
        {
            ScopedModule kernel32(LoadLibraryW(L"kernel32.dll"));
            if (kernel32.Value == nullptr)
            {
                throw std::runtime_error(LastErrorText("LoadLibraryW kernel32.dll"));
            }

            auto loadLibrary = reinterpret_cast<LPTHREAD_START_ROUTINE>(GetProcAddress(kernel32.Value, "LoadLibraryW"));
            if (loadLibrary == nullptr)
            {
                throw std::runtime_error(LastErrorText("GetProcAddress LoadLibraryW"));
            }

            // kernel32 在每个进程中的映射地址一致，本地入口点可以直接给目标进程用。
            std::wstring path = Utf8ToWide(libraryPath);
            SIZE_T length = (path.size() + 1) * sizeof(wchar_t);
            void* remotePath = VirtualAllocEx(process, nullptr, length, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            if (remotePath == nullptr)
            {
                throw std::runtime_error(LastErrorText("VirtualAllocEx"));
            }

            try
            {
                if (WriteProcessMemory(process, remotePath, path.c_str(), length, nullptr) == FALSE)
                {
                    throw std::runtime_error(LastErrorText("WriteProcessMemory"));
                }

                HANDLE remoteThread = CreateRemoteThread(process, nullptr, 0, loadLibrary, remotePath, 0, nullptr);
                if (remoteThread == nullptr)
                {
                    throw std::runtime_error(LastErrorText("CreateRemoteThread"));
                }

                try
                {
                    if (WaitForSingleObject(remoteThread, kLoadLibraryTimeoutMs) != WAIT_OBJECT_0)
                    {
                        throw std::runtime_error("Timed out loading " + libraryPath);
                    }

                    DWORD exitCode = 0;
                    if (GetExitCodeThread(remoteThread, &exitCode) != FALSE && exitCode == 0)
                    {
                        throw std::runtime_error("LoadLibraryW failed in target process for " + libraryPath + ", missing dependency?");
                    }
                }
                catch (...)
                {
                    CloseHandle(remoteThread);
                    throw;
                }

                CloseHandle(remoteThread);
            }
            catch (...)
            {
                VirtualFreeEx(process, remotePath, 0, MEM_RELEASE);
                throw;
            }

            VirtualFreeEx(process, remotePath, 0, MEM_RELEASE);
        }

        void LoadLibraryUsingWindowHook(const std::string& libraryPath)
        {
            std::wstring path = Utf8ToWide(libraryPath);
            stubModule = LoadLibraryW(path.c_str());
            if (stubModule == nullptr)
            {
                throw std::runtime_error(LastErrorText("LoadLibraryW stub"));
            }

            auto windowProc = reinterpret_cast<HOOKPROC>(GetProcAddress(stubModule, "WndProc"));
            if (windowProc == nullptr)
            {
                throw std::runtime_error(LastErrorText("GetProcAddress WndProc"));
            }

            HWND targetWindow = WaitForWindowFromProcessId();
            DWORD threadId = GetWindowThreadProcessId(targetWindow, nullptr);
            if (threadId == 0)
            {
                throw std::runtime_error(LastErrorText("GetWindowThreadProcessId"));
            }

            windowHook = SetWindowsHookExW(WH_GETMESSAGE, windowProc, stubModule, threadId);
            if (windowHook == nullptr)
            {
                throw std::runtime_error(LastErrorText("SetWindowsHookEx"));
            }

            if (PostThreadMessageW(threadId, 0, 0, 0) == FALSE)
            {
                throw std::runtime_error(LastErrorText("PostThreadMessage"));
            }
        }

        [[nodiscard]] HWND WaitForWindowFromProcessId() const
        {
            for (int retry = 0; retry < kWindowRetryCount; ++retry)
            {
                HWND window = FindWindow();
                if (window != nullptr)
                {
                    return window;
                }

                DWORD exitCode = 0;
                if (GetExitCodeProcess(process, &exitCode) == FALSE)
                {
                    throw std::runtime_error(LastErrorText("GetExitCodeProcess"));
                }

                if (exitCode != kStillActive)
                {
                    throw std::runtime_error("Target process exited with code " + std::to_string(exitCode) + " before its window appeared");
                }

                Sleep(kWindowRetryDelayMs);
            }

            throw std::runtime_error("Timed out waiting for the target window");
        }

        [[nodiscard]] HWND FindWindow() const
        {
            struct Search
            {
                DWORD ProcessId;
                HWND Result;
            } search{ processId, nullptr };

            EnumWindows(
                [](HWND window, LPARAM parameter) -> BOOL
                {
                    auto* state = reinterpret_cast<Search*>(parameter);
                    DWORD owner = 0;
                    GetWindowThreadProcessId(window, &owner);
                    if (owner != state->ProcessId)
                    {
                        return TRUE;
                    }

                    state->Result = window;
                    return FALSE;
                },
                reinterpret_cast<LPARAM>(&search));

            return search.Result;
        }
    };

    void RunSession(HANDLE pipe)
    {
        std::unique_ptr<SuspendedProcess> process;

        while (true)
        {
            PacketHeader header{};
            if (!ReadExactly(pipe, &header, sizeof(header)))
            {
                break;
            }

            if (header.Type == PacketType::SessionTermination)
            {
                break;
            }

            if (header.Type != PacketType::Request)
            {
                continue;
            }

            switch (header.Command)
            {
                case PacketCommand::Create:
                {
                    std::vector<char> content = ReadContent(pipe, header);
                    std::string json(content.begin(), content.end());

                    ProcessStartInfo request;
                    request.ApplicationName = GetString(json, "ApplicationName");
                    request.CommandLine = GetString(json, "CommandLine");
                    request.CreationFlags = GetUInt32(json, "CreationFlags", 0);
                    request.CurrentDirectory = GetString(json, "CurrentDirectory");

                    process = std::make_unique<SuspendedProcess>();
                    process->Create(request);
                    WritePacket(pipe, PacketType::Response, PacketCommand::Create);
                    break;
                }

                case PacketCommand::StartProcess:
                {
                    std::string response;
                    if (process == nullptr)
                    {
                        response = "{\"Succeeded\":false,\"ErrorMessage\":\"Process not created\"}";
                    }
                    else
                    {
                        response = "{\"Succeeded\":true,\"ProcessId\":" + std::to_string(process->ProcessId()) + "}";
                    }

                    WritePacket(pipe, PacketType::Response, PacketCommand::StartProcess, response);
                    break;
                }

                case PacketCommand::LoadLibrary:
                {
                    std::string response;
                    try
                    {
                        std::vector<char> content = ReadContent(pipe, header);
                        std::string json(content.begin(), content.end());

                        LoadLibraryRequest request;
                        request.LibraryPath = GetString(json, "LibraryPath");

                        if (process == nullptr)
                        {
                            throw std::runtime_error("Process not created");
                        }

                        process->LoadLibraryIntoProcess(request);
                        response = "{\"Succeeded\":true}";
                    }
                    catch (const std::exception& exception)
                    {
                        response = "{\"Succeeded\":false,\"ErrorMessage\":\"" + EscapeJson(exception.what()) + "\"}";
                    }

                    WritePacket(pipe, PacketType::Response, PacketCommand::LoadLibrary, response);
                    break;
                }

                case PacketCommand::ResumeMainThread:
                {
                    std::string response;
                    try
                    {
                        if (process == nullptr)
                        {
                            throw std::runtime_error("Process not created");
                        }

                        process->ResumeMainThread();
                        response = "{\"Succeeded\":true}";
                    }
                    catch (const std::exception& exception)
                    {
                        response = "{\"Succeeded\":false,\"ErrorMessage\":\"" + EscapeJson(exception.what()) + "\"}";
                    }

                    WritePacket(pipe, PacketType::Response, PacketCommand::ResumeMainThread, response);
                    break;
                }

                default:
                    break;
            }
        }
    }
}

int WINAPI wWinMain(HINSTANCE, HINSTANCE, PWSTR, int)
{
    HANDLE pipe = CreateNamedPipeW(
        kPipeName,
        PIPE_ACCESS_DUPLEX,
        PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
        1,
        65536,
        65536,
        0,
        nullptr);

    if (pipe == INVALID_HANDLE_VALUE)
    {
        return 1;
    }

    if (ConnectNamedPipe(pipe, nullptr) == FALSE && GetLastError() != ERROR_PIPE_CONNECTED)
    {
        CloseHandle(pipe);
        return 1;
    }

    try
    {
        RunSession(pipe);
    }
    catch (const PipeException&)
    {
    }

    FlushFileBuffers(pipe);
    DisconnectNamedPipe(pipe);
    CloseHandle(pipe);
    return 0;
}
