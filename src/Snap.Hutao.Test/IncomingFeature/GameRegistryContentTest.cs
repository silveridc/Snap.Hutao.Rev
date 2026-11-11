using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;

namespace Snap.Hutao.Test.IncomingFeature;

[TestClass]
public class GameRegistryContentTest
{
    private const string MiHoYoRegistryRoot = @"Software\miHoYo";

    private static readonly JsonSerializerOptions RegistryContentSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    /// <summary>
    /// 导出国服/国际服客户端在注册表里留下的内容，供排查安装路径与配置。
    /// 国服键名是「原神」、国际服是「Genshin Impact」，装了哪个导哪个，不要求两者同时存在。
    /// </summary>
    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void GetRegistryContent()
    {
        using RegistryKey hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
        using RegistryKey? miHoYoKey = hkcu.OpenSubKey(MiHoYoRegistryRoot);

        if (miHoYoKey is null)
        {
            Assert.Inconclusive($"本机未安装米哈游客户端：HKCU\\{MiHoYoRegistryRoot} 不存在");
            return;
        }

        string[] subkeys = miHoYoKey.GetSubKeyNames();
        Assert.IsTrue(subkeys.Length > 0, $"HKCU\\{MiHoYoRegistryRoot} 下没有任何子键");

        foreach (string subkey in subkeys)
        {
            DumpSubKey(miHoYoKey, subkey);
        }
    }

    [SupportedOSPlatform("windows")]
    private static void DumpSubKey(RegistryKey miHoYoKey, string subkeyName)
    {
        using RegistryKey? gameKey = miHoYoKey.OpenSubKey(subkeyName);
        if (gameKey is null)
        {
            return;
        }

        Dictionary<string, object> data = [];
        foreach (string valueName in gameKey.GetValueNames())
        {
            object? value = gameKey.GetValue(valueName);
            data[valueName] = gameKey.GetValueKind(valueName) switch
            {
                RegistryValueKind.DWord => (int)value!,
                RegistryValueKind.QWord => (long)value!,
                RegistryValueKind.Binary => GetStringOrObject((byte[])value!),
                RegistryValueKind.MultiString => string.Join("; ", (string[])value!),
                _ => value?.ToString() ?? string.Empty,
            };
        }

        Console.WriteLine($"Subkey: {subkeyName}");
        Console.WriteLine(JsonSerializer.Serialize(data, RegistryContentSerializerOptions));
    }

    private static object GetStringOrObject(byte[] bytes)
    {
        string temp = Encoding.UTF8.GetString(bytes).TrimEnd('\0');

        if (temp.Length > 0 && temp[0] is '{' or '[')
        {
            return JsonSerializer.Deserialize<JsonElement>(temp);
        }

        return temp;
    }
}
