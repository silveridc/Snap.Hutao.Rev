// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.IO.Hashing;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Web.Hutao;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Snap.Hutao.Rev.Service.Git;

internal static class RepositoryAffinity
{
    private static readonly Lock SyncRoot = new();

    public static ImmutableArray<GitRepository> Sort(ImmutableArray<GitRepository> repositories)
    {
        lock (SyncRoot)
        {
            int[] counts = new int[repositories.Length];
            for (int i = 0; i < repositories.Length; i++)
            {
                GitRepository repository = repositories[i];
                string key = RepositoryKey(repository.Name, repository.HttpsUrl.OriginalString);
                counts[i] = LocalSetting.Get(key, 0);
            }

            Array.Sort(counts, ImmutableCollectionsMarshal.AsArray(repositories));
            return repositories;
        }
    }

    public static void IncreaseFailure(GitRepository repository)
    {
        IncreaseFailure(repository.Name, repository.HttpsUrl.OriginalString);
    }

    public static void IncreaseFailure(string name, string url)
    {
        lock (SyncRoot)
        {
            string key = RepositoryKey(name, url);
            LocalSetting.Set(key, LocalSetting.Get(key, 0) + 1);
        }
    }

    public static void DecreaseFailure(GitRepository repository)
    {
        DecreaseFailure(repository.Name, repository.HttpsUrl.OriginalString);
    }

    public static void DecreaseFailure(string name, string url)
    {
        lock (SyncRoot)
        {
            string key = RepositoryKey(name, url);
            LocalSetting.Set(key, Math.Max(LocalSetting.Get(key, 0) - 1, 0));
        }
    }

    private static string RepositoryKey(string name, string url)
    {
        string hash = Hash.ToHexString(HashAlgorithmName.SHA256, $"{name}:{url.ToUpperInvariant()}");
        return $"Snap::Hutao::Git::RepositoryAffinity::{hash}";
    }
}
