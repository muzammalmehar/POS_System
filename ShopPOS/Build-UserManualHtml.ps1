$ErrorActionPreference = 'Stop'

$workspace = Split-Path -Parent $MyInvocation.MyCommand.Path
$markdownPath = Join-Path $workspace 'USER_MANUAL.md'
$htmlPath = Join-Path $workspace 'USER_MANUAL.html'
$shopName = 'Arslan Communication & Karyana Store'

$lines = Get-Content -Path $markdownPath

function Convert-InlineMarkdown {
    param([string]$Text)

    if ($null -eq $Text) {
        return ''
    }

    $encoded = [System.Net.WebUtility]::HtmlEncode($Text)
    $encoded = [System.Text.RegularExpressions.Regex]::Replace($encoded, '`([^`]+)`', '<code>$1</code>')
    return $encoded
}

$builder = New-Object System.Text.StringBuilder

[void]$builder.AppendLine('<!DOCTYPE html>')
[void]$builder.AppendLine('<html lang="en">')
[void]$builder.AppendLine('<head>')
[void]$builder.AppendLine('<meta charset="utf-8">')
[void]$builder.AppendLine('<meta name="viewport" content="width=device-width, initial-scale=1">')
[void]$builder.AppendLine("<title>$shopName User Manual</title>")
[void]$builder.AppendLine('<style>')
[void]$builder.AppendLine(@"
@page {
    size: A4;
    margin: 18mm 16mm 18mm 16mm;
}

* {
    box-sizing: border-box;
}

body {
    margin: 0;
    color: #1f2937;
    background: #eef2f7;
    font-family: "Segoe UI", Arial, sans-serif;
    font-size: 12pt;
    line-height: 1.55;
}

.sheet {
    width: 210mm;
    margin: 0 auto;
    background: #ffffff;
}

.hero {
    padding: 28px 34px 24px 34px;
    background: linear-gradient(135deg, #12346b 0%, #1f7a8c 100%);
    color: #ffffff;
}

.eyebrow {
    letter-spacing: 0.18em;
    text-transform: uppercase;
    font-size: 10pt;
    opacity: 0.9;
    margin-bottom: 10px;
}

.hero h1 {
    margin: 0;
    font-size: 28pt;
    line-height: 1.1;
}

.hero p {
    margin: 12px 0 0 0;
    max-width: 640px;
    font-size: 12pt;
    color: rgba(255, 255, 255, 0.92);
}

.content {
    padding: 26px 34px 32px 34px;
}

h2 {
    margin: 28px 0 10px 0;
    padding-bottom: 6px;
    border-bottom: 2px solid #d9e4f2;
    color: #12346b;
    font-size: 17pt;
    page-break-after: avoid;
}

h3 {
    margin: 16px 0 8px 0;
    color: #0f5f56;
    font-size: 12.5pt;
    page-break-after: avoid;
}

p {
    margin: 0 0 10px 0;
}

ul, ol {
    margin: 0 0 12px 20px;
    padding: 0;
}

li {
    margin: 0 0 6px 0;
}

code {
    padding: 1px 6px;
    border-radius: 999px;
    background: #eef4ff;
    color: #12346b;
    font-family: Consolas, "Courier New", monospace;
    font-size: 10.5pt;
}

blockquote, .note {
    margin: 14px 0;
    padding: 12px 14px;
    border-left: 4px solid #1f7a8c;
    background: #f4fbfb;
    border-radius: 8px;
}

.footer {
    margin-top: 28px;
    padding-top: 14px;
    border-top: 1px solid #d9e4f2;
    color: #6b7280;
    font-size: 10.5pt;
}

.page-break {
    page-break-before: always;
}
"@)
[void]$builder.AppendLine('</style>')
[void]$builder.AppendLine('</head>')
[void]$builder.AppendLine('<body>')
[void]$builder.AppendLine('<div class="sheet">')
[void]$builder.AppendLine('<section class="hero">')
[void]$builder.AppendLine('<div class="eyebrow">Operations Guide</div>')
[void]$builder.AppendLine("<h1>$shopName User Manual</h1>")
[void]$builder.AppendLine("<p>Complete operating guide for cashier, owner, and manager workflows at $shopName including sales, stock, credit, expiry, printing, vendors, services, and accounts.</p>")
[void]$builder.AppendLine('</section>')
[void]$builder.AppendLine('<main class="content">')

$inList = $false
$listType = ''

foreach ($rawLine in $lines) {
    $line = $rawLine.TrimEnd()
    $trimmed = $line.Trim()

    if ([string]::IsNullOrWhiteSpace($trimmed)) {
        if ($inList) {
            [void]$builder.AppendLine("</$listType>")
            $inList = $false
            $listType = ''
        }
        continue
    }

    if ($trimmed -match '^# (.+)$') {
        continue
    }

    if ($trimmed -match '^## (.+)$') {
        if ($inList) {
            [void]$builder.AppendLine("</$listType>")
            $inList = $false
            $listType = ''
        }
        [void]$builder.AppendLine("<h2>$(Convert-InlineMarkdown $Matches[1])</h2>")
        continue
    }

    if ($trimmed -match '^### (.+)$') {
        if ($inList) {
            [void]$builder.AppendLine("</$listType>")
            $inList = $false
            $listType = ''
        }
        [void]$builder.AppendLine("<h3>$(Convert-InlineMarkdown $Matches[1])</h3>")
        continue
    }

    if ($trimmed -match '^\- (.+)$') {
        if (-not $inList -or $listType -ne 'ul') {
            if ($inList) {
                [void]$builder.AppendLine("</$listType>")
            }
            [void]$builder.AppendLine('<ul>')
            $inList = $true
            $listType = 'ul'
        }
        [void]$builder.AppendLine("<li>$(Convert-InlineMarkdown $Matches[1])</li>")
        continue
    }

    if ($trimmed -match '^\d+\.\s+(.+)$') {
        if (-not $inList -or $listType -ne 'ol') {
            if ($inList) {
                [void]$builder.AppendLine("</$listType>")
            }
            [void]$builder.AppendLine('<ol>')
            $inList = $true
            $listType = 'ol'
        }
        [void]$builder.AppendLine("<li>$(Convert-InlineMarkdown $Matches[1])</li>")
        continue
    }

    if ($trimmed -eq '---') {
        if ($inList) {
            [void]$builder.AppendLine("</$listType>")
            $inList = $false
            $listType = ''
        }
        [void]$builder.AppendLine('<div class="footer"></div>')
        continue
    }

    if ($inList) {
        [void]$builder.AppendLine("</$listType>")
        $inList = $false
        $listType = ''
    }

    [void]$builder.AppendLine("<p>$(Convert-InlineMarkdown $trimmed)</p>")
}

if ($inList) {
    [void]$builder.AppendLine("</$listType>")
}

[void]$builder.AppendLine('<div class="footer">Prepared for ' + $shopName + ' as a complete day-to-day operating manual.</div>')
[void]$builder.AppendLine('</main>')
[void]$builder.AppendLine('</div>')
[void]$builder.AppendLine('</body>')
[void]$builder.AppendLine('</html>')

[System.IO.File]::WriteAllText($htmlPath, $builder.ToString(), [System.Text.Encoding]::UTF8)
Write-Output $htmlPath
