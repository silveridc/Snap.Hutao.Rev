// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal sealed class NameStringValue : NameValue<string>
{
    public NameStringValue(string name, string value)
        : base(name, value)
    {
    }

    public static NameStringValue Default { get; } = new(SH.ModelNameValueDefaultName, SH.ModelNameValueDefaultDescription);
}