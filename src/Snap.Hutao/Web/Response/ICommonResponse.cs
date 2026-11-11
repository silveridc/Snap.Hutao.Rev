// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using JetBrains.Annotations;

namespace Snap.Hutao.Rev.Web.Response;

internal interface ICommonResponse<[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)] out TResponse>
    where TResponse : ICommonResponse<TResponse>
{
    int ReturnCode { get; }

    string Message { get; set; }

    static abstract TResponse CreateDefault(int returnCode, string message);
}