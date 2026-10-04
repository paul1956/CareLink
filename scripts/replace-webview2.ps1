param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    [string]$RepoRoot = "$(PWD)"
)

Write-Host "Replacing committed WebView2 binaries with version $Version"

# Determine NuGet package root
$nugetRoot = $env:NUGET_PACKAGES
if ([string]::IsNullOrEmpty($nugetRoot)) {
    $nugetRoot = Join-Path -Path $env:USERPROFILE -ChildPath ".nuget\packages"
}

$packageId = "microsoft.web.webview2"
$managedPaths = @(
    Join-Path -Path $nugetRoot -ChildPath "$packageId\$Version\lib\net462\Microsoft.Web.WebView2.Core.dll",
    Join-Path -Path $nugetRoot -ChildPath "$packageId\$Version\lib\net45\Microsoft.Web.WebView2.Core.dll"
)
$loaderPath = Join-Path -Path $nugetRoot -ChildPath "$packageId\$Version\runtimes\win-x64\native\WebView2Loader.dll"

$outDir = Join-Path -Path $RepoRoot -ChildPath "src\CareLink\lib"
if (-not (Test-Path -Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }

$copied = $false
foreach ($mp in $managedPaths) {
    if (Test-Path -Path $mp) {
        Write-Host "Copying managed wrapper from: $mp"
        Copy-Item -Path $mp -Destination (Join-Path $outDir "Microsoft.Web.WebView2.Core.dll") -Force
        $copied = $true
        break
    }
}

if (-not $copied) {
    Write-Error "Managed wrapper not found in NuGet cache for version $Version. Checked: $($managedPaths -join '; ')"
    exit 1
}

if (Test-Path -Path $loaderPath) {
    Write-Host "Copying native loader from: $loaderPath"
    Copy-Item -Path $loaderPath -Destination (Join-Path $outDir "WebView2Loader.dll") -Force
} else {
    Write-Warning "Native loader not found at expected path: $loaderPath. Ensure runtime assets are available."
}

# Remove stale WebView2 DLLs from all bin/obj folders
Write-Host "Removing stale WebView2 DLLs from bin/obj folders..."
Get-ChildItem -Path $RepoRoot -Include bin,obj -Recurse -Directory -Force -ErrorAction SilentlyContinue |
    ForEach-Object {
        try {
            Get-ChildItem -Path $_.FullName -Filter "Microsoft.Web.WebView2*.dll" -Recurse -ErrorAction SilentlyContinue |
                ForEach-Object {
                    Write-Host "Removing: $($_.FullName)"
                    Remove-Item -Path $_.FullName -Force -ErrorAction SilentlyContinue
                }
        } catch { }
    }

Write-Host "Replace completed. Now run: `dotnet restore` and `dotnet build src\CareLink\CareLink.vbproj -c Debug -p:Platform=x64`"
exit 0
