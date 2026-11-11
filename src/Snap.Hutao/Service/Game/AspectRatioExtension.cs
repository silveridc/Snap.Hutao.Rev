// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.Property;
using System.Collections.Immutable;

namespace Snap.Hutao.Rev.Service.Game;

internal static class AspectRatioExtension
{
    extension(IProperty<ImmutableArray<AspectRatio>> aspectRatios)
    {
        public ImmutableArray<AspectRatio> Add(AspectRatio aspectRatio)
        {
            if (!aspectRatios.Value.Contains(aspectRatio))
            {
                aspectRatios.Value = aspectRatios.Value.Add(aspectRatio);
            }

            return aspectRatios.Value;
        }

        public ImmutableArray<AspectRatio> Remove(AspectRatio aspectRatio)
        {
            return aspectRatios.Value = aspectRatios.Value.Remove(aspectRatio);
        }
    }
}