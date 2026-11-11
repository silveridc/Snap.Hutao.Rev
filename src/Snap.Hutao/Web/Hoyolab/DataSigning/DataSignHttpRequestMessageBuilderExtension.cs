// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Web.Request.Builder;

namespace Snap.Hutao.Rev.Web.Hoyolab.DataSigning;

internal static class DataSignHttpRequestMessageBuilderExtension
{
    extension(HttpRequestMessageBuilder builder)
    {
        public async ValueTask SignDataAsync(DataSignAlgorithmVersion version, SaltType saltType, bool includeChars)
        {
            DataSignOptions options = await DataSignOptions.CreateFromHttpRequestMessageBuilderAsync(builder, saltType, includeChars, version).ConfigureAwait(false);
            builder.SetHeader("DS", DataSignAlgorithm.GetDataSign(options));
        }
    }
}