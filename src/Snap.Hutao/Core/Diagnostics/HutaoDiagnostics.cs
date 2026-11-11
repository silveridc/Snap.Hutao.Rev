// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Model.Entity.Database;
using System.Security.Cryptography;
using System.Text;

namespace Snap.Hutao.Rev.Core.Diagnostics;

[Service(ServiceLifetime.Singleton, typeof(IHutaoDiagnostics))]
internal sealed partial class HutaoDiagnostics : IHutaoDiagnostics
{
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial HutaoDiagnostics(IServiceProvider serviceProvider);

    public IReadOnlyDictionary<string, object?> LocalSettings { get => LocalSetting.GetAll(); }

    public async ValueTask<int> ExecuteSqlAsync(string sql)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await appDbContext.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
        }
    }

    public Dictionary<string, string> MakeApplicationDataCompositeValue(params string[] values)
    {
        Dictionary<string, string> compositeValue = [];
        foreach (string value in values)
        {
            compositeValue.Add($"{CryptographicOperations.HashData(HashAlgorithmName.MD5, Encoding.UTF8.GetBytes(value)):X}", value);
        }

        return compositeValue;
    }
}
