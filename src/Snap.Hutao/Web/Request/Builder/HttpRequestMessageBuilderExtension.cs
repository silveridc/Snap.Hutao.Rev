// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Service.Notification;
using Snap.Hutao.Rev.Web.Response;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Snap.Hutao.Rev.Web.Request.Builder;

internal static class HttpRequestMessageBuilderExtension
{
    extension(HttpRequestMessageBuilder builder)
    {
        internal HttpRequestMessageBuilder Resurrect()
        {
            builder.HttpRequestMessage.Resurrect();
            return builder;
        }

        internal ValueTask<TypedHttpResponse<TResult>> SendAsync<TResult>(HttpClient httpClient, CancellationToken token)
            where TResult : class
        {
            return SendAsync<TResult>(builder, httpClient, HttpCompletionOption.ResponseContentRead, true, token);
        }

        internal ValueTask<TypedHttpResponse<TResult>> SendAsync<TResult>(HttpClient httpClient, bool logException, CancellationToken token)
            where TResult : class
        {
            return SendAsync<TResult>(builder, httpClient, HttpCompletionOption.ResponseContentRead, logException, token);
        }

        internal ValueTask<TypedHttpResponse<TResult>> SendAsync<TResult>(HttpClient httpClient, HttpCompletionOption completionOption, CancellationToken token)
            where TResult : class
        {
            return SendAsync<TResult>(builder, httpClient, completionOption, true, token);
        }

        internal async ValueTask<TypedHttpResponse<TResult>> SendAsync<TResult>(HttpClient httpClient, HttpCompletionOption completionOption, bool logException, CancellationToken token)
            where TResult : class
        {
            // 异常在 await 之后抛出，调用方栈会被截断，先在发起请求的位置留档
            StackTrace originStackTrace = new(true);

            HttpContext context = new()
            {
                HttpClient = httpClient,
                CompletionOption = completionOption,
                RequestAborted = token,
            };

            using (context)
            {
                await SendAsync(builder, context).ConfigureAwait(false);

                StringBuilder messageBuilder = new();
                Exception? caughtException = null;
                bool showInfo = true;

                try
                {
                    context.Exception?.Throw();
                    ArgumentNullException.ThrowIfNull(context.Response);

                    showInfo = false;
                    TResult? body = await builder.HttpContentSerializer.DeserializeAsync<TResult>(context.Response.Content, token).ConfigureAwait(false);
                    return new(context.Response.Headers, body);
                }
                catch (OperationCanceledException)
                {
                    showInfo = false;

                    // Populate to caller
                    throw;
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                    if (logException)
                    {
                        HttpRequestExceptionHandling.TryHandle(messageBuilder, builder, ex);
                    }

                    return new(context.Response?.Headers, default);
                }
                finally
                {
                    if (showInfo)
                    {
                        InfoBarMessage message = caughtException is null
                            ? InfoBarMessage.Error(messageBuilder.ToString())
                            : InfoBarMessage.ErrorWithDetails(messageBuilder.ToString(), caughtException, originStackTrace.ToString());
                        builder.ServiceProvider.GetRequiredService<IMessenger>().Send(message);
                    }
                }
            }
        }

        internal async ValueTask SendAsync(HttpContext context)
        {
            try
            {
                context.Request = builder.HttpRequestMessage;
                context.Response = await context.HttpClient.SendAsync(context.Request, context.CompletionOption, context.RequestAborted).ConfigureAwait(false);
                context.Response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                context.Exception = ExceptionDispatchInfo.Capture(ex);
            }
        }

        internal void Send(HttpClient httpClient)
        {
            try
            {
                using (httpClient.Send(builder.HttpRequestMessage))
                {
                }
            }
            catch (HttpRequestException)
            {
            }
            catch (IOException)
            {
            }
            catch (JsonException)
            {
            }
            catch (HttpContentSerializationException)
            {
            }
            catch (SocketException)
            {
            }
        }
    }
}