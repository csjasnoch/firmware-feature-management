# Firmware Feature Management - Database Reset Utility
# Removes the database file so it will be recreated with seed data on next launch

param(
    [switch]$Force
)

$ErrorActionPreference = "Continue"
$ProjectPath = Split-Path $PSScriptRoot -Parent
$DatabaseFile = Join-Path $ProjectPath "firmwarefeatures.db"
$DatabaseShmFile = Join-Path $ProjectPath "firmwarefeatures.db-shm"
$DatabaseWalFile = Join-Path $ProjectPath "firmwarefeatures.db-wal"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Database Reset Utility" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Check if database exists
if (-not (Test-Path $DatabaseFile)) {
    Write-Host "✓ No database file found. Nothing to reset." -ForegroundColor Green
    Write-Host "  Database will be created on next application launch." -ForegroundColor Gray
    exit 0
}

# Show database info
$dbSize = (Get-Item $DatabaseFile).Length / 1KB
Write-Host "Database found:" -ForegroundColor Yellow
Write-Host "  Path: $DatabaseFile" -ForegroundColor Gray
Write-Host "  Size: $([math]::Round($dbSize, 2)) KB" -ForegroundColor Gray
Write-Host ""

# Confirm if not forced
if (-not $Force) {
    Write-Host "⚠ WARNING: This will delete the database and all data!" -ForegroundColor Red
    Write-Host "  A new database with seed data will be created on next launch." -ForegroundColor Yellow
    Write-Host ""
    $response = Read-Host "Are you sure you want to continue? (yes/no)"
    
    if ($response -ne "yes") {
        Write-Host "`n✗ Reset cancelled." -ForegroundColor Yellow
        exit 0
    }
}

# Stop any running application processes on ports 5000 and 5001
Write-Host "`nStopping any running application processes..." -ForegroundColor Yellow
try {
    $processes5000 = Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue | 
        Select-Object -ExpandProperty OwningProcess -Unique
    $processes5001 = Get-NetTCPConnection -LocalPort 5001 -ErrorAction SilentlyContinue | 
        Select-Object -ExpandProperty OwningProcess -Unique
    
    $allProcesses = @($processes5000; $processes5001) | Where-Object { $_ } | Select-Object -Unique
    
    if ($allProcesses) {
        foreach ($proc in $allProcesses) {
            Stop-Process -Id $proc -Force -ErrorAction SilentlyContinue
            Write-Host "  Stopped process: PID $proc" -ForegroundColor Gray
        }
        Start-Sleep -Seconds 1
    }
    Write-Host "  ✓ Application processes stopped" -ForegroundColor Green
} catch {
    Write-Host "  ⚠ Could not check for running processes" -ForegroundColor Yellow
}

# Delete database files
Write-Host "`nDeleting database files..." -ForegroundColor Yellow
try {
    $filesDeleted = 0
    
    if (Test-Path $DatabaseFile) {
        Remove-Item $DatabaseFile -Force -ErrorAction Stop
        Write-Host "  ✓ Deleted: firmwarefeatures.db" -ForegroundColor Green
        $filesDeleted++
    }
    
    if (Test-Path $DatabaseShmFile) {
        Remove-Item $DatabaseShmFile -Force -ErrorAction SilentlyContinue
        Write-Host "  ✓ Deleted: firmwarefeatures.db-shm" -ForegroundColor Green
        $filesDeleted++
    }
    
    if (Test-Path $DatabaseWalFile) {
        Remove-Item $DatabaseWalFile -Force -ErrorAction SilentlyContinue
        Write-Host "  ✓ Deleted: firmwarefeatures.db-wal" -ForegroundColor Green
        $filesDeleted++
    }
    
    Write-Host "`n================================================" -ForegroundColor Green
    Write-Host "  ✓ Database reset complete!" -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Green
    Write-Host "  Deleted $filesDeleted file(s)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  Next steps:" -ForegroundColor Yellow
    Write-Host "    1. Launch the application:" -ForegroundColor Gray
    Write-Host "       .\Launch\Relaunch.ps1 -OpenBrowser" -ForegroundColor Cyan
    Write-Host "    2. A new database with seed data will be created" -ForegroundColor Gray
    Write-Host ""
    
} catch {
    Write-Host "`n✗ Error deleting database: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "  Tip: Make sure the application is not running" -ForegroundColor Yellow
    exit 1
}
