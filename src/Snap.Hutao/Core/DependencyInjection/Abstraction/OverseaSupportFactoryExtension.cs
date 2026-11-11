// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.ViewModel.User;

namespace Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

internal static class OverseaSupportFactoryExtension
{
    extension<TClient>(IOverseaSupportFactory<TClient> factory)
    {
        public TClient CreateFor(UserAndUid userAndUid)
        {
            return factory.Create(userAndUid.IsOversea);
        }
    }
}