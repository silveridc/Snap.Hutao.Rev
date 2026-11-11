// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Endpoint.Hutao;

internal interface IHomaRootAccess
{
    /// <summary>接口根地址，按 <see cref="ServerHosts"/> 的开关决定是否带 <c>/api</c>。</summary>
    string Root { get; }

    /// <summary>不带 <c>/api</c> 的接口根地址，给服务端没有包在 <c>/api</c> 下的路由用。</summary>
    string UnprefixedRoot => ServerHosts.Current.Api;
}