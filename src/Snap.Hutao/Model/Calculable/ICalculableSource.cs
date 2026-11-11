// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Calculable;

internal interface ICalculableSource<out TResult>
    where TResult : ICalculable
{
    public TResult ToCalculable();
}