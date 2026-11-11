// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model;

internal class NameValue<T> : IEquatable<NameValue<T>>
{
    public NameValue(string name, T value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }

    public T Value { get; }

    public bool Equals(NameValue<T>? other)
    {
        return other is not null
            && string.Equals(Name, other.Name)
            && EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }
}