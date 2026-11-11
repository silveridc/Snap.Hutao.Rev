// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Setting;
using Snap.Hutao.Rev.Web.Endpoint.Hutao;
using Snap.Hutao.Rev.Web.Request.Builder;
using Snap.Hutao.Rev.Web.Request.Builder.Abstraction;
using System.Net.Http.Headers;

namespace Snap.Hutao.Rev.ViewModel.Guide;

internal static class StaticResourceHttpHeaderBuilderExtension
{
    extension<TBuilder>(TBuilder builder)
        where TBuilder : class, IHttpHeadersBuilder<HttpHeaders>
    {
        public TBuilder SetStaticResourceControlHeaders()
        {
            return builder
                .SetHeader("x-hutao-quality", $"{UnsafeLocalSetting.Get(SettingKeys.StaticResourceImageQuality, StaticResourceQuality.Original)}")
                .SetHeader("x-hutao-archive", $"{UnsafeLocalSetting.Get(SettingKeys.StaticResourceImageArchive, StaticResourceArchive.Full)}");
        }
    }

    extension<TBuilder>(TBuilder builder)
        where TBuilder : class, IHttpHeadersBuilder<HttpHeaders>, IRequestUriBuilder
    {
        public TBuilder SetStaticResourceControlHeadersIfRequired()
        {
            return builder.RequestUri?.GetLeftPart(UriPartial.Authority).Equals(StaticResourcesEndpoints.Root, StringComparison.OrdinalIgnoreCase) is true
                ? builder.SetStaticResourceControlHeaders()
                : builder;
        }
    }
}