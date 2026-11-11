// Github:https://github.com/zhaoyj7 All rights reserved.
// Licensed under the MIT license

using Microsoft.UI.Xaml;

namespace Snap.Hutao.Deployment;

public sealed partial class App : Application
{
    public App() => InitializeComponent();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow window = new();
        window.Activate();
    }
}
