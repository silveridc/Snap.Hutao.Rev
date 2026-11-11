// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.DependencyInjection.Abstraction;

namespace Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord;

[Service(ServiceLifetime.Transient, typeof(IOverseaSupportFactory<IGameRecordClient>))]
internal sealed partial class GameRecordClientFactory : OverseaSupportFactory<IGameRecordClient, GameRecordClient, GameRecordClientOversea>
{
    [GeneratedConstructor(CallBaseConstructor = true)]
    public partial GameRecordClientFactory(IServiceProvider serviceProvider);
}