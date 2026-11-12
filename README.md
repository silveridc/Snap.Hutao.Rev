<!--<p align="center">
  <img src="https://github.com/user-attachments/assets/976e057c-f01e-486b-9fa0-04744ae96f99" alt="Snap Hutao Rev Banner" width="600"/>
</p>-->

<h1 align="center">Snap Hutao Rev</h1>

<p align="center">
  🎮 开源的原神工具箱，专为 Windows 平台设计，改善桌面端玩家的游戏体验<br/>
  🎮  An open-source Genshin Impact toolkit for Windows, designed to improve the desktop gaming experience
</p>

---

## 📖 简介 / Introduction

**中文**
Snap Hutao Rev 是 Snap Hutao 的fork，原项目上游仓库已不可访问，本仓库独立维护，并按 MIT 协议保留原版权声明。

**English**
Snap Hutao Rev is a fork of Snap Hutao. The original project's upstream repository is no longer accessible, so this repo is independently maintained and keeps the original copyright notice under the MIT license.

---

## 🚀 安装 / Installation

从本仓库的 Release 页面下载 MSIX 安装包，签名证书与包信息见对应 Release 说明。系统要求：Windows 10 1903+ / Windows 11，x64。

---

## 🛠️ 构建 / Build

- 环境：Windows 10 1903+ / 11 x64，Visual Studio 2026（.NET 10 + Windows App SDK 工作负载）
- 打开解决方案 [`src/Snap.Hutao.slnx`](src/Snap.Hutao.slnx)，平台选择 `x64`，F5 即可构建调试
- 打包与 CI 使用同一套 Cake 脚本：`dotnet tool restore && dotnet cake`
- 注意：命令行构建前请先关闭 Visual Studio，避免 `obj` 目录竞争导致的 `WMC0001` 一类构建错误

手动构建与提交规范见 [CONTRIBUTING.md](CONTRIBUTING.md)。

---

## 📁 目录结构 / Layout

| 路径 | 说明 |
|---|---|
| `src/Snap.Hutao` | WinUI 3 客户端 |
| `src/Snap.Hutao.Test` | 单元测试 |
| `src/Snap.Hutao.Deployment` | 自更新器 |
| `src/Snap.Hutao.Native` | FullTrust 注入、Island 等原生组件 |
| `ServerHosts.cs` | 后端地址配置 |
| `build.cake` | 打包、生成清单与签名流程 |
| `res` | Logo 与仓库素材 |

---

## 🙏 致谢 / Special Thanks

- 原项目 Snap Hutao 及 [DGP Studio](https://github.com/DGP-Studio)
- [HolographicHat](https://github.com/HolographicHat)
- [UIGF organization](https://uigf.org)
- [genshin-fps-unlock](https://github.com/34736384/genshin-fps-unlock)
- [Snap.Hutao.Remastered.UnlockerIsland](https://github.com/SnapHutaoRemasteringProject/Snap.Hutao.Remastered.UnlockerIsland)
---

## ⚙️ 技术栈 / Tech Stack

- [CommunityToolkit/dotnet](https://github.com/CommunityToolkit/dotnet)
- [CommunityToolkit/Labs-Windows](https://github.com/CommunityToolkit/Labs-Windows)
- [CommunityToolkit/Windows](https://github.com/CommunityToolkit/Windows)
- [dotnet/efcore](https://github.com/dotnet/efcore)
- [dotnet/runtime](https://github.com/dotnet/runtime)
- [microsoft/vs-validation](https://github.com/microsoft/vs-validation)
- [microsoft/WindowsAppSDK](https://github.com/microsoft/WindowsAppSDK)
- [microsoft/microsoft-ui-xaml](https://github.com/microsoft/microsoft-ui-xaml)
- [quartznet/quartznet](https://github.com/quartznet/quartznet)

---

## 📜 许可 / License

本项目基于 MIT 协议发布，完整文本见 [LICENSE.txt](LICENSE.txt)。原始版权归 DGP Studio 所有，本分支的修改版权归 zhaoyj 所有。
