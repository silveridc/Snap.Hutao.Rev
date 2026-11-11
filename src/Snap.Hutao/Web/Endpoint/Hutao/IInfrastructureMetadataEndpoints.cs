// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.Globalization;

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IInfrastructureMetadataEndpoints : IInfrastructureRootAccess
{
    // string MetadataStatic(string locale, string name) => $"{ServerHosts.Current.Infrastructure}/metadata/Genshin/{locale}/{name}.json";
    string MetadataStatic(string locale, string name) => $"{ServerHosts.Current.Infrastructure}/metadata/Genshin/{locale}/{name}.json";

    string Metadata(string locale, string fileName) => $"{Root}/metadata/Genshin/{locale}/{fileName}";

    string Metadata(string template, string locale, string fileName) => string.Format(CultureInfo.CurrentCulture, template, $"Genshin/{locale}/{fileName}");

    string MetadataTemplate() => $"{Root}/metadata/template";
}