// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.IO;
using System.Text.Json.Nodes;

namespace Snap.Hutao.Rev.Core.Setting;

internal static class LocalSetting
{
    private static readonly Lock SyncRoot = new();

    private static string SettingsFilePath
    {
        get
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Snap.Hutao", "LocalCache");
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, "settings.json");
        }
    }

    public static T Get<T>(string key, T defaultValue = default!)
    {
        lock (SyncRoot)
        {
            JsonObject root = Load();
            if (root.TryGetPropertyValue(key, out JsonNode? node) && node is not null)
            {
                return node.Deserialize<T>() ?? defaultValue;
            }

            root[key] = JsonSerializer.SerializeToNode(defaultValue);
            Save(root);
            return defaultValue;
        }
    }

    public static void Set<T>(string key, T value)
    {
        lock (SyncRoot)
        {
            JsonObject root = Load();
            root[key] = JsonSerializer.SerializeToNode(value);
            Save(root);
        }
    }

    public static void SetIf<T>(bool condition, string key, T value)
    {
        if (condition)
        {
            Set(key, value);
        }
    }

    public static void SetIfNot<T>(bool condition, string key, T value)
    {
        if (!condition)
        {
            Set(key, value);
        }
    }

    public static T Update<T>(string key, T defaultValue, Func<T, T> modifier)
    {
        T oldValue = Get(key, defaultValue);
        Set(key, modifier(oldValue));
        return oldValue;
    }

    public static T Update<T>(string key, T defaultValue, T newValue)
    {
        T oldValue = Get(key, defaultValue);
        Set(key, newValue);
        return oldValue;
    }

    [SuppressMessage("", "SH008")]
    public static IReadOnlyDictionary<string, object?> GetAll()
    {
        lock (SyncRoot)
        {
            JsonObject root = Load();
            return root.ToDictionary(static kvp => kvp.Key, static kvp => (object?)kvp.Value?.DeepClone());
        }
    }

    private static JsonObject Load()
    {
        try
        {
            if (File.Exists(SettingsFilePath))
            {
                string json = File.ReadAllText(SettingsFilePath);
                if (JsonNode.Parse(json) is JsonObject root)
                {
                    return root;
                }
            }
        }
        catch
        {
            // Corrupted settings file, fall back to empty
        }

        return [];
    }

    private static void Save(JsonObject root)
    {
        string json = root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsFilePath, json);
    }
}
