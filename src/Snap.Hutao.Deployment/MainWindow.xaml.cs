// Github:https://github.com/zhaoyj7 All rights reserved.
// Licensed under the MIT license

using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Reflection;
using Windows.Graphics;
using Ellipse = Microsoft.UI.Xaml.Shapes.Ellipse;

namespace Snap.Hutao.Deployment;

public sealed partial class MainWindow : Window, IDisposable
{
    private const string IconResourceName = "Snap.Hutao.Deployment.Assets.Installer.ico";
    private const string TitleIconResourceName = "Snap.Hutao.Deployment.Assets.Logo.ico";
    private const string ArtworkResourceName = "Snap.Hutao.Deployment.Assets.Artwork.png";
    private const string IconFileName = "Snap.Hutao.Deployment.Installer.ico";
    private const string TitleIconFileName = "Snap.Hutao.Deployment.Logo.ico";
    private const string ArtworkFileName = "Snap.Hutao.Deployment.Artwork.png";
    private const int WindowWidth = 780;
    private const int WindowHeight = 540;

    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    private readonly (Ellipse Dot, FontIcon Check, ProgressRing Ring, TextBlock Text)[] steps;
    private bool started;
    private bool finished;
    private int currentStep = -1;

    public MainWindow()
    {
        InitializeComponent();
        steps =
        [
            (StepConnectDot, StepConnectDone, StepConnectRing, StepConnectText),
            (StepDownloadDot, StepDownloadDone, StepDownloadRing, StepDownloadText),
            (StepVerifyDot, StepVerifyDone, StepVerifyRing, StepVerifyText),
            (StepInstallDot, StepInstallDone, StepInstallRing, StepInstallText),
        ];

        ActionButton.Click += OnActionButtonClick;
        Activated += OnActivated;
        Closed += (_, _) => Dispose();

        ResizeWindow();
        ApplyImages();
    }

    public void Dispose() => cancellationTokenSource.Dispose();

    private void ResizeWindow()
    {
        AppWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = false;
            presenter.IsMaximizable = false;
        }
    }

    // 单文件发布下资源不在磁盘上，而 AppWindow.SetIcon 与 BitmapImage 的 file URI 都只认路径，
    // 所以把内嵌资源落到临时目录再喂给它们。
    private void ApplyImages()
    {
        string? iconPath = ExtractResource(IconResourceName, IconFileName);
        if (iconPath is not null)
        {
            AppWindow.SetIcon(iconPath);
        }

        string? titleIconPath = ExtractResource(TitleIconResourceName, TitleIconFileName);
        if (titleIconPath is not null)
        {
            TitleImage.Source = new BitmapImage(new Uri(titleIconPath));
        }

        string? artworkPath = ExtractResource(ArtworkResourceName, ArtworkFileName);
        if (artworkPath is not null)
        {
            ArtworkImage.Source = new BitmapImage(new Uri(artworkPath));
        }
    }

    private static string? ExtractResource(string resourceName, string fileName)
    {
        Stream? resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (resource is null)
        {
            return null;
        }

        string path = Path.Combine(Path.GetTempPath(), fileName);
        using (resource)
        using (FileStream file = File.Create(path))
        {
            resource.CopyTo(file);
        }

        return path;
    }

    private void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        if (started)
        {
            return;
        }

        started = true;
        _ = UpdateAsync();
    }

    private async Task UpdateAsync()
    {
        Progress<UpdateState> progress = new(Apply);
        try
        {
            // 这里不能 ConfigureAwait(false)：Progress<T> 会把下载途中的回调投递回 UI 线程，
            // 但收尾的 Apply 是直调，必须留在 UI 线程上，否则跨线程访问 Xaml 对象的异常会被这个 fire-and-forget 任务吞掉。
            await UpdaterFlow.RunAsync(progress, cancellationTokenSource.Token);
            Apply(new UpdateState(UpdateStage.Completed, "胡桃启动器即将重新打开", string.Empty, 100d, false));
            CloseAfter(TimeSpan.FromSeconds(5));
        }
        catch (OperationCanceledException)
        {
            Apply(new UpdateState(UpdateStage.Canceled, "下载已中止，当前版本不受影响", string.Empty, null, false));
        }
        catch (Exception ex)
        {
            Apply(new UpdateState(UpdateStage.Failed, ex.Message, string.Empty, null, false));
            ErrorTextBlock.Text = "请稍后在胡桃启动器里重新触发更新，或检查服务端发布的镜像地址是否可匿名下载。";
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
        finally
        {
            finished = true;
            ActionButton.Content = "关闭";
            ActionButton.IsEnabled = true;
        }
    }

    private void Apply(UpdateState state)
    {
        HeaderTextBlock.Text = state.Stage switch
        {
            UpdateStage.Completed => "更新完成",
            UpdateStage.Canceled => "更新已取消",
            UpdateStage.Failed => "更新失败",
            _ => "正在更新胡桃启动器",
        };

        DetailTextBlock.Text = state.Detail;
        StatusTextBlock.Text = state.Status;
        StatusTextBlock.Visibility = string.IsNullOrEmpty(state.Status) ? Visibility.Collapsed : Visibility.Visible;

        if (state.Version is not null)
        {
            VersionTextBlock.Text = $"胡桃启动器 v{state.Version}";
        }

        if (!string.IsNullOrWhiteSpace(state.ReleaseNotes))
        {
            ReleaseNotesTextBlock.Text = state.ReleaseNotes;
            ReleaseNotesPanel.Visibility = Visibility.Visible;
        }

        int step = state.Stage switch
        {
            UpdateStage.Connecting => 0,
            UpdateStage.Downloading => 1,
            UpdateStage.Verifying => 2,
            UpdateStage.Installing => 3,
            UpdateStage.Completed => steps.Length,
            _ => currentStep,
        };

        if (step >= 0)
        {
            currentStep = step;
            for (int i = 0; i < steps.Length; i++)
            {
                ApplyStep(i, done: step > i, active: step == i);
            }
        }

        if (state.Percent is double percent)
        {
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Value = percent;
        }
        else if (state.Stage is UpdateStage.Failed or UpdateStage.Canceled)
        {
            ProgressBar.IsIndeterminate = false;
        }
        else
        {
            ProgressBar.IsIndeterminate = true;
        }

        // 失败/取消后不再保留转圈，步骤停在已经走到的那一步
        if (state.Stage is UpdateStage.Failed or UpdateStage.Canceled)
        {
            foreach ((_, _, ProgressRing ring, _) in steps)
            {
                ring.IsActive = false;
                ring.Visibility = Visibility.Collapsed;
            }
        }

        ActionButton.IsEnabled = state.Cancellable;
    }

    private void ApplyStep(int index, bool done, bool active)
    {
        (Ellipse dot, FontIcon check, ProgressRing ring, TextBlock text) = steps[index];
        dot.Visibility = done || active ? Visibility.Collapsed : Visibility.Visible;
        check.Visibility = done ? Visibility.Visible : Visibility.Collapsed;
        ring.Visibility = active ? Visibility.Visible : Visibility.Collapsed;
        ring.IsActive = active;
        text.Opacity = done ? 0.8 : active ? 1d : 0.5;
        text.FontSize = done ? 13 : 14;
    }

    private void OnActionButtonClick(object sender, RoutedEventArgs e)
    {
        if (finished)
        {
            Close();
            return;
        }

        cancellationTokenSource.Cancel();
        ActionButton.IsEnabled = false;
    }

    private void CloseAfter(TimeSpan delay)
    {
        DispatcherQueueTimer timer = dispatcherQueue.CreateTimer();
        timer.Interval = delay;
        timer.IsRepeating = false;
        timer.Tick += (_, _) => Close();
        timer.Start();
    }
}
