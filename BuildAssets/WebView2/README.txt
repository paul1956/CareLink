Place the WebView2 bootstrapper (MicrosoftEdgeWebView2RuntimeInstallerX64.exe) in this folder.

Recommended options:
- Bootstrapper (small downloader, per-user install): MicrosoftEdgeWebView2RuntimeInstallerX64.exe
- Standalone MSI (offline/enterprise): MicrosoftEdgeWebView2RuntimeInstallerX64.msi

Project notes (current configuration):
- The project now targets Microsoft.Web.WebView2 version 1.0.818.41. The managed wrapper (Microsoft.Web.WebView2.Core.dll) placed into the app output must match this package version.
- The MSBuild targets copy the WebView2 managed DLL and the x64 native WebView2Loader.dll from the NuGet package cache into the project's build output. If you clear bin manually, rebuild to restore these files.
- The bootstrapper in this folder is copied into the application output (bin\<Configuration>) when present so it will be included in ZIP deployments.

Recommended files to include in ZIP distributions (from the app output folder):
- Microsoft.Web.WebView2.Core.dll (managed wrapper) - ensure this matches package version 1.0.818.41
- WebView2Loader.dll (native loader, x64)
- MicrosoftEdgeWebView2RuntimeInstallerX64.exe (bootstrapper) - optional but recommended to help non-dev users install the runtime

Recommended workflow:
1. Run the helper script to download the bootstrapper into this folder if not present: install-bootstrapper.ps1
   pwsh -ExecutionPolicy Bypass -File .\BuildAssets\WebView2\install-bootstrapper.ps1
2. Rebuild the project so MSBuild copies the managed/native WebView2 DLLs into bin. If you cleared bin, run a rebuild.
3. Zip the contents of the app output folder (for example: src\CareLink\bin\Debug\net11.0-windows10.0.17763.0) - this ensures the correct DLLs and the installer are included.

If you need to change the WebView2 package version used by the project, update the PackageReference in src\CareLink\CareLink.vbproj and rebuild; then ensure the ZIP contains the matching Microsoft.Web.WebView2.Core.dll.

Official downloads and details: https://developer.microsoft.com/microsoft-edge/webview2/#download-section

If you want, the build can be made tolerant of a missing bootstrapper by making the copy conditional in the project file; currently the repo copies the installer only when present.
