// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Core.Property;

internal sealed partial class ForceDisabledPropertyWrapper : IObservableProperty<bool>
{
    private readonly IObservableProperty<bool> source;
    private readonly IReadOnlyObservableProperty<bool> isDisabled;

    public ForceDisabledPropertyWrapper(IObservableProperty<bool> source, IReadOnlyObservableProperty<bool> isDisabled)
    {
        this.source = source;
        this.isDisabled = isDisabled;
        source.PropertyChanged += OnSourcePropertyChanged;
        isDisabled.PropertyChanged += OnSourcePropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool Value
    {
        get => !isDisabled.Value && source.Value;
        set => source.Value = value;
    }

    public INotifyPropertyChangedDeferral GetDeferral()
    {
        return source.GetDeferral();
    }

    private void OnSourcePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // 统一以 "Value" 广播：包装后的值同时取决于两个输入
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }
}
