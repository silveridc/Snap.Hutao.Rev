// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.WinUI.Helpers;
using JetBrains.Annotations;

namespace Snap.Hutao.Rev.Extension;

internal static class NotifyPropertyChangedExtension
{
    extension(INotifyPropertyChanged source)
    {
        public WeakEventListener<TInstance, object?, PropertyChangedEventArgs> WeakPropertyChanged<TInstance>(TInstance instance, [RequireStaticDelegate] Action<TInstance, object?, PropertyChangedEventArgs> callback)
            where TInstance : class
        {
            WeakEventListener<TInstance, object?, PropertyChangedEventArgs> weakEvent = new(instance)
            {
                OnEventAction = callback,
                OnDetachAction = listener => source.PropertyChanged -= listener.OnEvent,
            };

            source.PropertyChanged += weakEvent.OnEvent;
            return weakEvent;
        }
    }
}