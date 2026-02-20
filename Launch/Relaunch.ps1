# Firmware Feature Management - Relaunch Script
# Stops, cleans, rebuilds, and relaunches the application

param(
    [switch]$SkipClean,
    [switch]$SkipBuild,
    [switch]$OpenBrowser,
    [int]$Port = 5000,
    [string]$BrowserUrl = ""
)

$ErrorActionPreference = "Continue"
$ProjectPath = Split-Path $PSScriptRoot -Parent
$ProjectFile = "WebApp.csproj"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Firmware Feature Management - Relaunch" -ForegroundColor Cyan
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
                $process = Get-Process -Id $proc -ErrorAction SilentlyContinue
                if ($process) {
                    Write-Host "  Stopping process: $($process.Name) (PID: $proc)" -ForegroundColor Yellow
                    Stop-Process -Id $proc -Force -ErrorAction SilentlyContinue
                    Start-Sleep -Milliseconds 500
                }
            }
            Write-Host "  ✓ Processes stopped" -ForegroundColor Green
        } else {
            Write-Host "  ✓ No processes found on port $PortNumber" -ForegroundColor Green
        }
    } catch {
        Write-Host "  ⚠ Could not check port (may already be free)" -ForegroundColor Yellow
    }
}

# Function to clean build artifacts
function Clean-BuildArtifacts {
    Write-Host "`nCleaning build artifacts..." -ForegroundColor Yellow
    
    try {
        # Remove bin and obj directories
        $dirsToRemove = @("bin", "obj", "publish")
        foreach ($dir in $dirsToRemove) {
            $fullPath = Join-Path $ProjectPath $dir
            if (Test-Path $fullPath) {
                Write-Host "  Removing $dir..." -ForegroundColor Gray
                Remove-Item $fullPath -Recurse -Force -ErrorAction SilentlyContinue
            }
        }
        
        # Remove database file if exists (will be recreated with seed data)
        $dbFile = Join-Path $ProjectPath "firmwarefeatures.db"
        if (Test-Path $dbFile) {
            Write-Host "  Removing database file (will be recreated)..." -ForegroundColor Gray
            Remove-Item $dbFile -Force -ErrorAction SilentlyContinue
        }
        
        Write-Host "  ✓ Clean completed" -ForegroundColor Green
        return $true
    } catch {
        Write-Host "  ✗ Clean failed: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Function to restore NuGet packages
function Restore-Packages {
    Write-Host "`nRestoring NuGet packages..." -ForegroundColor Yellow
    
    try {
        $output = dotnet restore "$ProjectPath\$ProjectFile" 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ Restore completed" -ForegroundColor Green
            return $true
        } else {
            Write-Host "  ✗ Restore failed" -ForegroundColor Red
            Write-Host $output -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "  ✗ Restore failed: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Function to build project
function Build-Project {
    Write-Host "`nBuilding project..." -ForegroundColor Yellow
    
    try {
        $output = dotnet build "$ProjectPath\$ProjectFile" --configuration Debug 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ Build completed" -ForegroundColor Green
            return $true
        } else {
            Write-Host "  ✗ Build failed" -ForegroundColor Red
            Write-Host $output -ForegroundColor Red
            
            # Check for common issues
            if ($output -match "error CS") {
                Write-Host "`n  Common fixes:" -ForegroundColor Yellow
                Write-Host "    1. Check for syntax errors in recently modified files" -ForegroundColor Gray
                Write-Host "    2. Ensure all using statements are present" -ForegroundColor Gray
                Write-Host "    3. Verify all referenced types exist" -ForegroundColor Gray
            }
            
            return $false
        }
    } catch {
        Write-Host "  ✗ Build failed: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Function to check .NET SDK
function Test-DotNetSDK {
    Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
    
    try {
        $dotnetVersion = dotnet --version 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ .NET SDK version: $dotnetVersion" -ForegroundColor Green
            return $true
        } else {
            Write-Host "  ✗ .NET SDK not found" -ForegroundColor Red
            Write-Host "    Please install .NET SDK from https://dotnet.microsoft.com/download" -ForegroundColor Yellow
            return $false
        }
    } catch {
        Write-Host "  ✗ .NET SDK not found" -ForegroundColor Red
        return $false
    }
}

# Main execution
try {
    # Check .NET SDK
    if (-not (Test-DotNetSDK)) {
        exit 1
    }
    
    # Stop existing processes
    Stop-ProcessOnPort $Port
    Stop-ProcessOnPort 5001
    
    # Clean if not skipped
    if (-not $SkipClean) {
        if (-not (Clean-BuildArtifacts)) {
            Write-Host "`n⚠ Clean failed but continuing..." -ForegroundColor Yellow
        }
    } else {
        Write-Host "`nSkipping clean (use without -SkipClean to clean)" -ForegroundColor Gray
    }
    
    # Restore packages
    if (-not (Restore-Packages)) {
        Write-Host "`n✗ Failed to restore packages. Exiting." -ForegroundColor Red
        exit 1
    }
    
    # Build if not skipped
    if (-not $SkipBuild) {
        if (-not (Build-Project)) {
            Write-Host "`n✗ Build failed. Exiting." -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "`nSkipping build (use without -SkipBuild to build)" -ForegroundColor Gray
    }
    
    # Determine browser URL
    $finalUrl = if ($BrowserUrl) { $BrowserUrl } else { "http://localhost:$Port" }
    
    # Launch application
    Write-Host "`n================================================" -ForegroundColor Green
    Write-Host "   Launching application..." -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Green
    Write-Host "  HTTP:  http://localhost:$Port" -ForegroundColor Cyan
    Write-Host "  HTTPS: https://localhost:5001" -ForegroundColor Cyan
    Write-Host ""
    
    if ($OpenBrowser) {
        Write-Host "Opening browser to: $finalUrl" -ForegroundColor Yellow
        Start-Sleep -Seconds 2
        Start-Process $finalUrl
    }
    
    # Navigate to project directory and run
    Set-Location $ProjectPath
    dotnet run --project $ProjectFile --urls "http://localhost:$Port;https://localhost:5001"
    
} catch {
    Write-Host "`n✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} finally {
    Write-Host "`nShutdown complete" -ForegroundColor Cyan
}
