param(
    [int] $Size = 64,
    [string] $OutDir = "src\CareLink\Images\Flags",
    [switch] $AlsoCopyToRuntime
)

# Download a full set of country flags (ISO 3166-1 alpha-2) from FlagCDN
# Usage:
#   pwsh.exe -NoProfile -ExecutionPolicy Bypass -File .\scripts\download-all-flags.ps1 -Size 64
# Set -AlsoCopyToRuntime to also copy the files to the runtime flags folder next to the exe

$codes = @(
    'ad','ae','af','ag','ai','al','am','ao','aq','ar','as','at','au','aw','ax','az',
    'ba','bb','bd','be','bf','bg','bh','bi','bj','bl','bm','bn','bo','bq','br','bs','bt','bv','bw','by','bz',
    'ca','cc','cd','cf','cg','ch','ci','ck','cl','cm','cn','co','cr','cu','cv','cw','cx','cy','cz',
    'de','dj','dk','dm','do','dz','ec','ee','eg','eh','er','es','et','fi','fj','fk','fm','fo','fr',
    'ga','gb','gd','ge','gf','gg','gh','gi','gl','gm','gn','gp','gq','gr','gs','gt','gu','gw','gy',
    'hk','hm','hn','hr','ht','hu','id','ie','il','im','in','io','iq','ir','is','it','je','jm','jo','jp',
    'ke','kg','kh','ki','km','kn','kp','kr','kw','ky','kz','la','lb','lc','li','lk','lr','ls','lt','lu','lv','ly',
    'ma','mc','md','me','mf','mg','mh','mk','ml','mm','mn','mo','mp','mq','mr','ms','mt','mu','mv','mw','mx','my','mz',
    'na','nc','ne','nf','ng','ni','nl','no','np','nr','nu','nz','om','pa','pe','pf','pg','ph','pk','pl','pm','pn','pr','ps','pt','pw','py','qa',
    're','ro','rs','ru','rw','sa','sb','sc','sd','se','sg','sh','si','sj','sk','sl','sm','sn','so','sr','ss','st','sv','sx','sy','sz',
    'tc','td','tf','tg','th','tj','tk','tl','tm','tn','to','tr','tt','tv','tw','tz','ua','ug','um','us','uy','uz',
    'va','vc','ve','vg','vi','vn','vu','wf','ws','xk','ye','yt','za','zm','zw'
)

$repoRoot = (Get-Location).Path
$outPath = Join-Path -Path $repoRoot -ChildPath $OutDir
if (-not (Test-Path $outPath)) {
    New-Item -ItemType Directory -Path $outPath -Force | Out-Null
}

$flagCdnBase = "https://flagcdn.com/w$Size"  # e.g. https://flagcdn.com/w64/us.png

Write-Host "Downloading $($codes.Count) flags to $outPath (size: $Size)"

foreach ($code in $codes) {
    $codeLower = $code.ToLower()
    $dest = Join-Path -Path $outPath -ChildPath ("$codeLower.png")
    Write-Host "Downloading $codeLower..."
    $succeeded = $false

    # Try multiple FlagCDN sizes/endpoints in case one is unavailable
    $flagCdnSizes = @($Size, 160, 320)
    foreach ($sz in $flagCdnSizes) {
        $url = "https://flagcdn.com/w$sz/$codeLower.png"
        try {
            Invoke-WebRequest -Uri $url -OutFile $dest -UseBasicParsing -ErrorAction Stop
            Write-Host "Saved $codeLower from FlagCDN (w$sz)"
            $succeeded = $true
            break
        } catch {
            Write-Verbose (("FlagCDN w{0} failed for {1}: {2}") -f $sz, $codeLower, $_)
        }
    }

    if (-not $succeeded) {
        # Fallback to Twemoji (raw GitHub). Convert ISO code to regional indicator codepoints.
        try {
            Function CodeToTwemojiName([string] $iso) {
                $iso = $iso.ToUpper()
                $chars = $iso.ToCharArray()
                $parts = @()
                foreach ($c in $chars) {
                    $delta = [int][char]$c - [int][char]'A'
                    $cp = 0x1F1E6 + $delta
                    $parts += ('{0:x}' -f $cp)
                }
                return ($parts -join '-')
            }

            $twName = CodeToTwemojiName($codeLower)
            $twUrl = "https://raw.githubusercontent.com/twitter/twemoji/master/assets/72x72/$twName.png"
            Invoke-WebRequest -Uri $twUrl -OutFile $dest -UseBasicParsing -ErrorAction Stop
            Write-Host "Saved $codeLower from Twemoji"
            $succeeded = $true
        } catch {
            Write-Warning ("Failed to download $codeLower from FlagCDN and Twemoji: {0}" -f $_)
        }
    }
}

Write-Host "Download complete. Files saved to: $outPath"

if ($AlsoCopyToRuntime) {
    # Copy to runtime flags folder next to executable
    try {
        $buildOutput = Join-Path -Path $repoRoot -ChildPath "bin\Debug\net11.0-windows"
        if (-not (Test-Path $buildOutput)) {
            $buildOutput = Join-Path -Path $repoRoot -ChildPath "bin\Debug\net10.0-windows"
        }
        $runtimeFlags = Join-Path -Path $buildOutput -ChildPath "flags"
        if (-not (Test-Path $runtimeFlags)) { New-Item -ItemType Directory -Path $runtimeFlags -Force | Out-Null }
        Copy-Item -Path (Join-Path $outPath "*.png") -Destination $runtimeFlags -Force
        Write-Host "Copied flags to runtime folder: $runtimeFlags"
    } catch {
        Write-Warning "Failed to copy to runtime folder: $_"
    }
}
