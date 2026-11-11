// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Model;

namespace Snap.Hutao.Rev.ViewModel.AvatarProperty;

internal abstract class NameIconDescription : NameDescription, INameIcon<Uri>
{
    public Uri Icon { get; set; } = default!;
}