# Downloads the WebView2 bootstrapper into this BuildAssets\WebView2 folder if it is not present.
param(
    [string]$Destination = "$PSScriptRoot\MicrosoftEdgeWebView2RuntimeInstallerX64.exe"
)

if (Test-Path -Path $Destination) {
    Write-Output "Bootstrapper already present: $Destination"
    exit 0
}

$downloadUrl = 'https://go.microsoft.com/fwlink/p/?LinkId=2124703' # official WebView2 Evergreen Bootstrapper x64
try {
    Write-Output "Downloading WebView2 bootstrapper from $downloadUrl ..."
    Invoke-WebRequest -Uri $downloadUrl -OutFile $Destination -UseBasicParsing
    Write-Output "Downloaded to $Destination"
} catch {
    Write-Error "Failed to download bootstrapper: $_"
    exit 1
}

Write-Output "Bootstrapper is ready. Build the project to copy it into the bin output."