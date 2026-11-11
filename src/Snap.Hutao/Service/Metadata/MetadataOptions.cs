// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core;
using System.IO;

namespace Snap.Hutao.Rev.Service.Metadata;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class MetadataOptions
{
    private readonly CultureOptions cultureOptions;

    [GeneratedConstructor]
    public partial MetadataOptions(IServiceProvider serviceProvider);

    public string LocaleName { get => cultureOptions.LocaleName; }

    public string LanguageCode { get => cultureOptions.LanguageCode; }

    [field: MaybeNull]
    public string LocalizedDataFolder
    {
        get
        {
            if (field is null)
            {
                field = Path.Combine(HutaoRuntime.DataDirectory, "Metadata", cultureOptions.LocaleName);
                Directory.CreateDirectory(field);
            }

            return field;
        }
    }

    public string GetLocalizedLocalPath(string fileNameWithExtension)
    {
        return Path.Combine(LocalizedDataFolder, fileNameWithExtension);
    }
}
