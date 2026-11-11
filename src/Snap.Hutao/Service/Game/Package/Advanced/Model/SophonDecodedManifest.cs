// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Hoyolab.Takumi.Downloader.Proto;

namespace Snap.Hutao.Rev.Service.Game.Package.Advanced.Model;

internal sealed class SophonDecodedManifest
{
    public SophonDecodedManifest(string urlPrefix, string urlSuffix, SophonManifestProto sophonData)
    {
        UrlPrefix = string.Intern(urlPrefix);
        UrlSuffix = string.IsNullOrEmpty(urlSuffix) ? string.Empty : string.Intern($"?{urlSuffix}");
        Data = sophonData;
    }

    public string UrlPrefix { get; }

    public string UrlSuffix { get; }

    public SophonManifestProto Data { get; }
}