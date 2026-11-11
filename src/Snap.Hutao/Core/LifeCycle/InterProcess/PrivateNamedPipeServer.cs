// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.BetterGenshinImpact;
using Snap.Hutao.Rev.Core.LifeCycle.InterProcess.Model;
using Snap.Hutao.Rev.Core.Security.Principal;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;

namespace Snap.Hutao.Rev.Core.LifeCycle.InterProcess;

[Service(ServiceLifetime.Singleton)]
internal sealed partial class PrivateNamedPipeServer : IDisposable
{
    private readonly BetterGenshinImpactNamedPipeServer betterGenshinImpactNamedPipeServer;
    private readonly PrivateNamedPipeMessageDispatcher messageDispatcher;
    private readonly ILogger<PrivateNamedPipeServer> logger;

    private readonly NamedPipeServerStream serverStream = CreatePipeServerStream();
    private readonly CancellationTokenSource serverTokenSource = new();
    private readonly AsyncLock serverLock = new();

    [GeneratedConstructor]
    public partial PrivateNamedPipeServer(IServiceProvider serviceProvider);

    public void Dispose()
    {
        serverTokenSource.Cancel();

        // 先关闭流以中断可能阻塞中的同步读取，再有限等待后台循环退出，避免退出流程卡死
        serverStream.Dispose();

        Task<AsyncLock.Releaser> lockTask = serverLock.LockAsync();
        if (lockTask.Wait(TimeSpan.FromSeconds(3)))
        {
            lockTask.Result.Dispose();
            serverTokenSource.Dispose();
        }
    }

    public void Start()
    {
        RunAsync().SafeForget();
    }

    private static NamedPipeServerStream CreatePipeServerStream()
    {
        PipeSecurity pipeSecurity = new();
        pipeSecurity.AddAccessRule(new(SecurityIdentifiers.Everyone, PipeAccessRights.FullControl, AccessControlType.Allow));

        return NamedPipeServerStreamAcl.Create(
            PrivateNamedPipe.PrivateName,
            PipeDirection.InOut,
            NamedPipeServerStream.MaxAllowedServerInstances,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous | PipeOptions.WriteThrough,
            0,
            0,
            pipeSecurity);
    }

    private async ValueTask RunAsync()
    {
        using (await serverLock.LockAsync().ConfigureAwait(false))
        {
            while (!serverTokenSource.IsCancellationRequested)
            {
                try
                {
                    await serverStream.WaitForConnectionAsync(serverTokenSource.Token).ConfigureAwait(false);
                    logger.LogInformation("Pipe session created");
                    RunPacketSession(serverStream, serverTokenSource.Token);
                }
                catch (IOException)
                {
                    try
                    {
                        serverStream.Disconnect();
                    }
                    catch
                    {
                        // Ignored
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    // Dispose 会先关闭流来中断阻塞中的读取
                    break;
                }
            }
        }
    }

    private void RunPacketSession(NamedPipeServerStream serverStream, CancellationToken token)
    {
        while (serverStream.IsConnected && !token.IsCancellationRequested)
        {
            serverStream.ReadPacket(out PipePacketHeader header);
            logger.LogInformation("Pipe packet: [Type:{Type}] [Command:{Command}]", header.Type, header.Command);
            switch (header.Type, header.Command)
            {
                case (PipePacketType.Request, PipePacketCommand.RequestElevationStatus):
                    ElevationStatusResponse resp = new(HutaoRuntime.IsProcessElevated, Environment.ProcessId);
                    serverStream.WritePacketWithJsonContent(PrivateNamedPipe.PrivateVersion, PipePacketType.Response, PipePacketCommand.ResponseElevationStatus, resp);
                    serverStream.Flush();
                    break;

                case (PipePacketType.Request, PipePacketCommand.RedirectActivation):
                    HutaoActivationArguments? hutaoArgs = serverStream.ReadJsonContent<HutaoActivationArguments>(in header);
                    if (hutaoArgs is not null)
                    {
                        logger.LogInformation("Redirect activation: [Kind:{Kind}] [Arguments:{Arguments}]", hutaoArgs.Kind, hutaoArgs.LaunchActivatedArguments);
                    }

                    messageDispatcher.RedirectedActivation(hutaoArgs);
                    break;

                case (PipePacketType.Request, PipePacketCommand.BetterGenshinImpactToSnapHutaoRequest):
                    PipeRequest<JsonElement>? request = serverStream.ReadJsonContent<PipeRequest<JsonElement>>(in header);
                    PipeResponse response = betterGenshinImpactNamedPipeServer.DispatchRequest(request);
                    serverStream.WritePacketWithJsonContent(PrivateNamedPipe.PrivateVersion, PipePacketType.Response, PipePacketCommand.SnapHutaoToBetterGenshinImpactResponse, response);
                    serverStream.Flush();
                    break;

                case (PipePacketType.SessionTermination, _):
                    serverStream.Disconnect();
                    if (header.Command is PipePacketCommand.Exit)
                    {
                        messageDispatcher.ExitApplication();
                    }

                    return;
            }
        }
    }
}