// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev;

/// <summary>
/// 后端地址。测试时把 <see cref="Current"/> 改成另一套即可，重新编译生效。
/// </summary>
internal static class ServerHosts
{
    public static Host Current => Production;

    /// <summary>自建线上。</summary>
    public static Host Production => new("https://snap-api.sliveridc1.cn", "https://snap.sliveridc1.cn", "http://sh-doc.zhaoyj7.internal");

    /// <summary>测试服务端，三个地址按自己的环境改。</summary>
    public static Host Test => new("https://127.0.0.1:5004", "http://127.0.0.1:5888", "http://127.0.0.1:5889");

    /// <summary>服务端地址</summary>
    /// <param name="Api">接口主机地址，不含 <c>/api</c> 前缀。</param>
    /// <param name="Infrastructure">静态资源、元数据根地址。</param>
    /// <param name="Doc">文档站根地址，目录结构与 <c>doc/dist</c> 一致（反馈页与更新日志从这里加载）。</param>
    public sealed record Host(string Api, string Infrastructure, string Doc)
    {
        public string ApiRoot => $"{Api}/api";
    }
}
