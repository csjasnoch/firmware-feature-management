# Firmware Feature Management - Relaunch Script
param([switch]$SkipClean, [switch]$SkipBuild, [switch]$OpenBrowser, [int]$Port = 5000)

$ErrorActionPreference = "Continue"
$ProjectPath = $PSScriptRoot
$ProjectFile = "WebApp.csproj"

Write-Host "Firmware Feature Management - Relaunch" -ForegroundColor Cyan

function Stop-ProcessOnPort([int]$PortNumber) {
    Write-Host "Checking port $PortNumber..." -ForegroundColor Yellow
    try {
        $processes = Get-NetTCPConnection -LocalPort $PortNumber -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess -Unique
        if ($processes) {
            foreach ($proc in $processes) {
                Stop-Process -Id $proc -Force -ErrorAction SilentlyContinue
            }
            Write-Host "  Processes stopped" -ForegroundColor Green
        }
    } catch { }
}

function Clean-BuildArtifacts {
    Write-Host "Cleaning..." -ForegroundColor Yellow
    try {
        @("bin", "obj", "publish") | ForEach-Object {
            $path = Join-Path $ProjectPath $_
            if (Test-Path $path) { Remove-Item $path -Recurse -Force -ErrorAction SilentlyContinue }
        }
        $dbFile = Join-Path $ProjectPath "firmwarefeatures.db"
        if (Test-Path $dbFile) { Remove-Item $dbFile -Force -ErrorAction SilentlyContinue }
        Write-Host "  Clean OK" -ForegroundColor Green
        return $true
    } catch { return $false }
}

function Restore-Packages {
    Write-Host "Restoring..." -ForegroundColor Yellow
    dotnet restore "$ProjectPath\$ProjectFile" 2>&1 | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Restore OK" -ForegroundColor Green
        return $true
    }
    Write-Host "  Restore failed" -ForegroundColor Red
    return $false
}

function Build-Project {
    Write-Host "Building..." -ForegroundColor Yellow
    dotnet build "$ProjectPath\$ProjectFile" --configuration Debug 2>&1 | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Build OK" -ForegroundColor Green
        return $true
    }
    Write-Host "  Build failed" -ForegroundColor Red
    return $false
}

try {
    Stop-ProcessOnPort $Port
    Stop-ProcessOnPort 5001
    
    if (-not $SkipClean) { Clean-BuildArtifacts | Out-Null }
    if (-not (Restore-Packages)) { exit 1 }
    if (-not $SkipBuild -and -not (Build-Project)) { exit 1 }
    
    Write-Host "Launching at http://localhost:$Port" -ForegroundColor Green
    if ($OpenBrowser) { Start-Process "http://localhost:$Port" }
    
    Set-Location $ProjectPath
    dotnet run --project $ProjectFile --urls "http://localhost:$Port;https://localhost:5001"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} finally {
    Write-Host "Shutdown complete" -ForegroundColor Cyan
}
