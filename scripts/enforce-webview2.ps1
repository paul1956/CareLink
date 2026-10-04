param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    [Parameter(Mandatory=$true)]
    [string]$OutputPath
)
Write-Host "Enforcing WebView2 version: $Version in output: $OutputPath"

try {
    $resolved = $null
    try {
        $resolved = Resolve-Path -Path $OutputPath -ErrorAction Stop
    } catch {
        # Try combining with script directory if OutputPath is relative
        $combined = Join-Path -Path (Get-Location) -ChildPath $OutputPath
        try { $resolved = Resolve-Path -Path $combined -ErrorAction Stop } catch { }
    }

    if (-not $resolved) {
        Write-Error "OutputPath not found: $OutputPath"
        exit 1
    }

    $outPath = $resolved.ProviderPath
    Write-Host "Resolved output path: $outPath"

    $files = Get-ChildItem -Path $outPath -Filter 'Microsoft.Web.WebView2*.dll' -Recurse -ErrorAction SilentlyContinue
    $bad = $false

    foreach ($f in $files) {
        try {
            $asm = [Reflection.AssemblyName]::GetAssemblyName($f.FullName)
            $ver = $asm.Version.ToString()
            if ($ver -ne $Version) {
                Write-Host "MISMATCH: $($f.FullName) -> $ver"
                $bad = $true
            } else {
                Write-Host "OK: $($f.FullName) -> $ver"
            }
        } catch {
            Write-Host "NON-MANAGED OR UNREADABLE: $($f.FullName) - $($_.Exception.Message)"
            $bad = $true
        }
    }

    if ($bad) {
        Write-Error "WebView2 version mismatch detected; ensure all WebView2 assemblies match $Version"
        exit 1
    } else {
        Write-Host "WebView2 versions OK: $Version"
        exit 0
    }
} catch {
    Write-Error "Unexpected error while enforcing WebView2 version: $($_.Exception.Message)"
    exit 1
}
