cmake_minimum_required(VERSION 3.20)

project(Snap.Hutao.Native LANGUAGES CXX)

# 只做 x64：启动器、游戏、YaeAchievementLib 都是 x64
if (NOT CMAKE_SIZEOF_VOID_P EQUAL 8)
    message(FATAL_ERROR "Snap.Hutao.Native must be built for x64")
endif ()

add_library(Snap.Hutao.Native.Ext SHARED
    DllInjectionUtilities.cpp
)

set_target_properties(Snap.Hutao.Native.Ext PROPERTIES
    OUTPUT_NAME "Snap.Hutao.Native.Ext"
    CXX_STANDARD 20
    CXX_STANDARD_REQUIRED ON
)

target_compile_definitions(Snap.Hutao.Native.Ext PRIVATE
    UNICODE
    _UNICODE
    WIN32_LEAN_AND_MEAN
    NOMINMAX
)

if (MSVC)
    target_compile_options(Snap.Hutao.Native.Ext PRIVATE /W4 /permissive- /utf-8)
    # 静态链接 CRT，避免再带一个 VCRUNTIME140 依赖
    set_property(TARGET Snap.Hutao.Native.Ext PROPERTY MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")

    # Release 也保留符号，调试注入流程时可以直接单步进原生代码
    # /DEBUG 会把 /OPT 的默认值改成 NOREF，这里显式补回裁剪，否则静态 CRT 会整包塞进来
    target_compile_options(Snap.Hutao.Native.Ext PRIVATE $<$<CONFIG:Release>:/Zi>)
    target_link_options(Snap.Hutao.Native.Ext PRIVATE $<$<CONFIG:Release>:/DEBUG /OPT:REF /OPT:ICF>)
endif ()

# 构建完直接刷新随包资源；出包时 cake 会先跑这个任务
add_custom_command(TARGET Snap.Hutao.Native.Ext POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E make_directory
        "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/Native"
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "$<TARGET_FILE:Snap.Hutao.Native.Ext>"
        "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/Native/Snap.Hutao.Native.Ext.dll"
)

if (MSVC)
    # .pdb 会被 MSIX 打包无条件剔除（只进符号包），所以随包副本换个后缀，运行时再还原
    add_custom_command(TARGET Snap.Hutao.Native.Ext POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_if_different
            "$<TARGET_PDB_FILE:Snap.Hutao.Native.Ext>"
            "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/Native/Snap.Hutao.Native.Ext.pdb.dat"
    )
endif ()

# 桌面快捷方式的提权引导程序：以 runas 重新激活 shell:AppsFolder\<家族名>!App
add_executable(Snap.Hutao.Elevated.Launcher WIN32
    ElevatedLauncher.cpp
)

set_target_properties(Snap.Hutao.Elevated.Launcher PROPERTIES
    OUTPUT_NAME "Snap.Hutao.Elevated.Launcher"
    CXX_STANDARD 20
    CXX_STANDARD_REQUIRED ON
)

target_compile_definitions(Snap.Hutao.Elevated.Launcher PRIVATE
    UNICODE
    _UNICODE
    WIN32_LEAN_AND_MEAN
    NOMINMAX
)

target_link_libraries(Snap.Hutao.Elevated.Launcher PRIVATE Shell32)

if (MSVC)
    target_compile_options(Snap.Hutao.Elevated.Launcher PRIVATE /W4 /permissive- /utf-8)
    # 静态链接 CRT，快捷方式在任何机器上都能直接启动
    set_property(TARGET Snap.Hutao.Elevated.Launcher PROPERTY MSVC_RUNTIME_LIBRARY "MultiThreaded")
endif ()

add_custom_command(TARGET Snap.Hutao.Elevated.Launcher POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E make_directory
        "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/Native"
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "$<TARGET_FILE:Snap.Hutao.Elevated.Launcher>"
        "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/Native/Snap.Hutao.Elevated.Launcher.exe"
)

# 提权助手：挂起创建游戏、注入 DLL、恢复主线程；纯 Win32，静态 CRT，体积远小于替代掉的自包含 .NET 版本
add_executable(Snap.Hutao.FullTrust WIN32 FullTrust.cpp)
set_target_properties(Snap.Hutao.FullTrust PROPERTIES
    OUTPUT_NAME "Snap.Hutao.FullTrust"
    CXX_STANDARD 20
    CXX_STANDARD_REQUIRED ON
)

target_compile_definitions(Snap.Hutao.FullTrust PRIVATE
    UNICODE
    _UNICODE
    WIN32_LEAN_AND_MEAN
    NOMINMAX
)

target_link_libraries(Snap.Hutao.FullTrust PRIVATE user32)

if (MSVC)
    target_compile_options(Snap.Hutao.FullTrust PRIVATE /W4 /permissive- /utf-8 $<$<CONFIG:Release>:/GL>)
    set_property(TARGET Snap.Hutao.FullTrust PROPERTY MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
    target_link_options(Snap.Hutao.FullTrust PRIVATE $<$<CONFIG:Release>:/LTCG /OPT:REF /OPT:ICF>)
endif ()

add_custom_command(TARGET Snap.Hutao.FullTrust POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E make_directory
        "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/FpsUnlock"
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "$<TARGET_FILE:Snap.Hutao.FullTrust>"
        "${CMAKE_CURRENT_SOURCE_DIR}/../Snap.Hutao/Resource/FpsUnlock/Snap.Hutao.FullTrust.exe"
)
