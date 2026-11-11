// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.ExceptionService;

namespace Snap.Hutao.Rev.Core.Threading;

internal static class TaskExtension
{
    extension(Task task)
    {
        public async void SafeForget()
        {
            try
            {
                await task.ConfigureAwait(true);
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
            catch (Exception ex)
            {
                ExceptionHandling.KillProcessOnDbExceptionNoThrow(ex);
                ex.SetSentryMechanism("TaskExtension.SafeForget", handled: true);
                SentrySdk.CaptureException(ex);
            }
        }
    }

    extension(ValueTask task)
    {
        public async void SafeForget()
        {
            try
            {
                await task.ConfigureAwait(true);
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
            catch (Exception ex)
            {
                ExceptionHandling.KillProcessOnDbExceptionNoThrow(ex);
                ex.SetSentryMechanism("TaskExtension.SafeForget", handled: true);
                SentrySdk.CaptureException(ex);
            }
        }
    }
}