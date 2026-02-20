# Firmware Feature Management - Quick Launch Script
# Fast restart without cleaning or rebuilding (for rapid development iteration)

param(
    [switch]$OpenBrowser,
    [int]$Port = 5000,
    [string]$BrowserUrl = ""
)

$ErrorActionPreference = "Continue"
$ProjectPath = Split-Path $PSScriptRoot -Parent
$ProjectFile = "WebApp.csproj"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Quick Launch (Skip Clean & Build)" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Function to kill processes on port
function Stop-ProcessOnPort {
    param([int]$PortNumber)
    
    Write-Host "Checking for processes on port $PortNumber..." -ForegroundColor Yellow
    
    try {
        $processes = Get-NetTCPConnection -LocalPort $PortNumber -ErrorAction SilentlyContinue | 
            Select-Object -ExpandProperty OwningProcess -Unique
        
        if ($processes) {
            foreach ($proc in $processes) {
                Stop-Process -Id $proc -Force -ErrorAction SilentlyContinue
                Start-Sleep -Milliseconds 300
            }
            Write-Host "  ✓ Processes stopped" -ForegroundColor Green
        } else {
            Write-Host "  ✓ Port is free" -ForegroundColor Green
        }
    } catch {
        Write-Host "  ⚠ Could not check port" -ForegroundColor Yellow
    }
}

try {
    # Stop existing processes
    Stop-ProcessOnPort $Port
    Stop-ProcessOnPort 5001
    
    # Determine browser URL
    $finalUrl = if ($BrowserUrl) { $BrowserUrl } else { "http://localhost:$Port" }
    
    # Launch application
    Write-Host "`n================================================" -ForegroundColor Green
    Write-Host "   Launching application (no build)..." -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Green
    Write-Host "  HTTP:  http://localhost:$Port" -ForegroundColor Cyan
    Write-Host "  HTTPS: https://localhost:5001" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  ⚠ Note: Code changes require a full rebuild" -ForegroundColor Yellow
    Write-Host "     Use Relaunch.ps1 for full rebuild" -ForegroundColor Gray
    Write-Host ""
    
    if ($OpenBrowser) {
        Write-Host "Opening browser to: $finalUrl" -ForegroundColor Yellow
        Start-Sleep -Seconds 1
        Start-Process $finalUrl
    }
    
    # Navigate to project directory and run
    Set-Location $ProjectPath
    dotnet run --project $ProjectFile --urls "http://localhost:$Port;https://localhost:5001" --no-build
    
} catch {
    Write-Host "`n✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "  Tip: If this fails, you may need to run a full build first:" -ForegroundColor Yellow
    Write-Host "       .\Launch\Relaunch.ps1" -ForegroundColor Gray
    exit 1
} finally {
    Write-Host "`nShutdown complete" -ForegroundColor Cyan
}
