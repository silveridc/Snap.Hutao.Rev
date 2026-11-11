// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Property;

internal interface IObservableProperty<T> : IProperty<T>, INotifyPropertyChanged
{
    INotifyPropertyChangedDeferral GetDeferral();
}