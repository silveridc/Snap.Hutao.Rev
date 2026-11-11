// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Response;

internal interface ICommonResponseValidator<in TResponse>
    where TResponse : ICommonResponse<TResponse>
{
    bool TryValidate(TResponse response);

    bool TryValidateWithoutUINotification(TResponse response);
}
