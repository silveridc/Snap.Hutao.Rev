// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Metadata.Converter;

internal interface IIconNameToUriConverter
{
    static abstract Uri IconNameToUri(string name);
}