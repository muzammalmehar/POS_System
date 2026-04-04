$ErrorActionPreference = 'Stop'

$workspace = Split-Path -Parent $MyInvocation.MyCommand.Path
$builderScript = Join-Path $workspace 'Build-UserManualHtml.ps1'
$htmlPath = & $builderScript
$pdfPath = Join-Path $workspace 'Arslan_Communication_And_Karyana_Store_User_Manual.pdf'

$chromeCandidates = @(
    'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe',
    'C:\Program Files\Microsoft\Edge\Application\msedge.exe',
    'C:\Program Files\Google\Chrome\Application\chrome.exe',
    'C:\Program Files (x86)\Google\Chrome\Application\chrome.exe'
)

$browserPath = $chromeCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $browserPath) {
    throw 'No supported browser found for PDF export.'
}

$htmlUri = ([System.Uri]$htmlPath).AbsoluteUri
$tempProfile = Join-Path $env:TEMP ('ShopPOSPdfProfile_' + [Guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $tempProfile -Force
$arguments = @(
    '--headless',
    '--disable-gpu',
    '--user-data-dir=' + $tempProfile,
    '--print-to-pdf=' + $pdfPath,
    '--print-to-pdf-no-header',
    $htmlUri
)

$startInfo = New-Object System.Diagnostics.ProcessStartInfo
$startInfo.FileName = $browserPath
$startInfo.UseShellExecute = $false
$startInfo.CreateNoWindow = $true
$startInfo.Arguments = (($arguments | ForEach-Object { '"' + ($_ -replace '"', '\"') + '"' }) -join ' ')

$process = New-Object System.Diagnostics.Process
$process.StartInfo = $startInfo
[void]$process.Start()
$process.WaitForExit()

if ($process.ExitCode -ne 0) {
    throw "PDF export failed with exit code $($process.ExitCode)."
}

Remove-Item -LiteralPath $tempProfile -Recurse -Force -ErrorAction SilentlyContinue

Write-Output $pdfPath
