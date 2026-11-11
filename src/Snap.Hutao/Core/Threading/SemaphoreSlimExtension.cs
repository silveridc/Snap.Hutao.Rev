// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.ExceptionService;

namespace Snap.Hutao.Rev.Core.Threading;

internal static class SemaphoreSlimExtension
{
    extension(SemaphoreSlim semaphoreSlim)
    {
        public ValueTask<SemaphoreSlimToken> EnterAsync(CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<SemaphoreSlimToken>(token);
            }

            try
            {
                if (semaphoreSlim.Wait(0))
                {
                    return new(new SemaphoreSlimToken(semaphoreSlim));
                }
            }
            catch (ObjectDisposedException ex)
            {
                HutaoException.OperationCanceled(SH.CoreThreadingSemaphoreSlimDisposed, ex);
            }

            return WaitAndEnterAsync(semaphoreSlim, token);

            static async ValueTask<SemaphoreSlimToken> WaitAndEnterAsync(SemaphoreSlim semaphoreSlim, CancellationToken token)
            {
                try
                {
                    await semaphoreSlim.WaitAsync(token).ConfigureAwait(false);
                }
                catch (ObjectDisposedException ex)
                {
                    HutaoException.OperationCanceled(SH.CoreThreadingSemaphoreSlimDisposed, ex);
                }

                return new(semaphoreSlim);
            }
        }

        public SemaphoreSlimToken Enter()
        {
            try
            {
                semaphoreSlim.Wait();
            }
            catch (ObjectDisposedException ex)
            {
                HutaoException.OperationCanceled(SH.CoreThreadingSemaphoreSlimDisposed, ex);
            }

            return new(semaphoreSlim);
        }
    }
}