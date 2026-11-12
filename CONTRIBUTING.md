# Contribution Guide

## Contribute Your Code

### Setup Snap.Hutao Project

1. Download and install [Visual Studio 2026 Community](https://visualstudio.microsoft.com/downloads/).
   - In the Visual Studio Installer, ensure the following workloads are selected:
     - **.NET Desktop Development**
     - **Windows application development** (includes Windows App SDK and C# desktop templates)
2. Use Git to clone the project to your local device:
   ```bash
   git clone https://github.com/silveridc/Snap.Hutao.Rev.git
   ```

3. Switch to the `develop` branch using Git.
4. Open `Snap.Hutao.sln` with Visual Studio 2026. Visual Studio will automatically restore NuGet packages and missing toolchains.
5. Set the startup project to `Snap.Hutao` and select your target architecture (e.g., `x64`), then build and run.

### Start Pull Request

* All contributions should target the `develop` branch of `silveridc/Snap.Hutao.Rev`.
* Please follow conventional commit guidelines and use [linking keywords](https://docs.github.com/en/get-started/writing-on-github/working-with-advanced-formatting/using-keywords-in-issues-and-pull-requests) (e.g., `Fixes #12`) to automatically close related issues when merged.

### Test Binary Package

Pre-release test packages and CI builds are published under the [Releases page](https://www.google.com/search?q=https://github.com/silveridc/Snap.Hutao.Rev/releases).

Since official signing certificates are deprecated, running unsigned/custom MSIX packages locally requires installing the self-signed test certificate:

1. Download the test package `.msix` and [Cert](https://sh-patch.cn-sn.bucket.silveridc.cn/certs/app.cer) from the target release assets.
2. Double-click `Snap.Hutao.Rev.cer` -> Click **Install Certificate** -> Choose **Local Machine**.
3. Place the certificate into the **Trusted People (受信任的人)** store.
4. Double-click the `.msix` file to install and launch.

## Start New Issue

Before opening a new issue, please verify whether the problem has already been reported in [Current Opened Issues](https://www.google.com/search?q=https://github.com/silveridc/Snap.Hutao.Rev/issues).

When reporting a bug or crash:

* Provide exact, step-by-step reproduction instructions.
* If the application crashes on startup or runtime:
* Check Windows Event Viewer under **Windows Logs > Application**.
* Provide the crash log and the `.NET Runtime` error stack trace.
