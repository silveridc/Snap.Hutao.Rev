// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Response;

internal interface ITypedResponseValidator<TData> : ICommonResponseValidator<Response<TData>>
{
    bool TryValidate(Response<TData> response, [NotNullWhen(true)] out TData? data);

    bool TryValidateWithoutUINotification(Response<TData> response, [NotNullWhen(true)] out TData? data);
}
