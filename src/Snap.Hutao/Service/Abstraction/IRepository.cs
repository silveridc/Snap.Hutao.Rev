// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Service.Abstraction;

internal interface IRepository<TEntity> : IAppInfrastructureService
    where TEntity : class;