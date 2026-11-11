// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Metadata;

internal sealed class ParameterDescription
{
    public ParameterDescription(string parameter, string description)
    {
        Parameter = parameter;
        Description = description;
    }

    public string Parameter { get; }

    public string Description { get; }
}