// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Property;

internal sealed partial class ObservablePropertyReadOnlyWrapper<T> : IReadOnlyObservableProperty<T>
{
    private readonly IObservableProperty<T> source;

    public ObservablePropertyReadOnlyWrapper(IObservableProperty<T> source)
    {
        this.source = source;
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => source.PropertyChanged += value;
        remove => source.PropertyChanged -= value;
    }

    public T Value
    {
        get => source.Value;
    }
}