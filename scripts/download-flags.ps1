param(
    [string[]] $Codes,
    [string] $OutDir = "src\CareLink\Images\Flags",
    [int] $Size = 64
)

# Downloads flag PNGs from flagcdn.com into the repo flags folder.
# Usage: .\download-flags.ps1 -Codes us,gb,dz -OutDir ..\src\CareLink\Images\Flags -Size 64

if (-not $Codes -or $Codes.Count -eq 0) {
    Write-Host "No codes specified. Read codes from codes.txt if present."
    $codesFile = Join-Path -Path (Get-Location) -ChildPath "codes.txt"
    if (Test-Path $codesFile) {
        $Codes = Get-Content $codesFile | Where-Object { $_ -and $_ -match "\w" } | ForEach-Object { $_.Trim().ToLower() }
    } else {
        Write-Host "No codes provided and codes.txt not found. Exiting."
        exit 1
    }
}

$repoRoot = (Get-Location).Path
$outPath = Join-Path -Path $repoRoot -ChildPath $OutDir
if (-not (Test-Path $outPath)) {
    New-Item -ItemType Directory -Path $outPath -Force | Out-Null
}

foreach ($c in $Codes) {
    $code = $c.ToLower()
    $url = "https://flagcdn.com/w$Size/$code.png"
    $dest = Join-Path -Path $outPath -ChildPath ("$code.png")
    try {
        Write-Host "Downloading $url -> $dest"
        Invoke-WebRequest -Uri $url -OutFile $dest -UseBasicParsing -ErrorAction Stop
    } catch {
        Write-Warning "Failed to download $code: $_"
    }
}

Write-Host "Done. Flags saved to $outPath"