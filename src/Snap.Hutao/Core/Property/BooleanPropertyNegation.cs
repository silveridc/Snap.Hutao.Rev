// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Property;

internal sealed class BooleanPropertyNegation : IProperty<bool>
{
    private readonly IProperty<bool> source;

    public BooleanPropertyNegation(IProperty<bool> source)
    {
        this.source = source;
    }

    public bool Value
    {
        get => !source.Value;
        set => source.Value = !value;
    }
}