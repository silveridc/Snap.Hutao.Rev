// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Response;

internal static class ResponseValidationServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddResponseValidation()
        {
            return services
                .AddTransient<ICommonResponseValidator<Response>, DefaultResponseValidator>()
                .AddTransient(typeof(ITypedResponseValidator<>), typeof(TypedResponseValidator<>));
        }
    }
}