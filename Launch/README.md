# Launch Scripts Documentation

This folder contains all the scripts needed to launch and manage the Firmware Feature Management application.

## Quick Start

### From Windows Explorer
**Double-click `Launch.bat`** in the root folder - this is the easiest way to start the application with automatic browser opening.

### From PowerShell

```powershell
# Full launch with clean, build, and browser
.\Launch\Relaunch.ps1 -OpenBrowser

# Quick restart (no rebuild) - fast for development
.\Launch\QuickLaunch.ps1 -OpenBrowser

# Reset database only
.\Launch\ResetDatabase.ps1
```

## Available Scripts

### `../Launch.bat` (Recommended Entry Point)
**Location:** Repository root  
**Purpose:** One-click launcher for Windows users

- Checks for .NET SDK installation
- Checks for PowerShell availability
- Launches the application with browser auto-open
- Shows errors if prerequisites are missing

**Usage:**
```cmd
Launch.bat
```

---

### `Relaunch.ps1` (Full Rebuild)
**Purpose:** Complete application restart with cleaning and rebuilding

**Features:**
- Stops any processes on ports 5000 and 5001
- Cleans build artifacts (`bin/`, `obj/`, `publish/`)
- Deletes and recreates database with seed data
- Restores NuGet packages
- Builds the project
- Launches application

**Parameters:**
- `-SkipClean` - Skip cleaning build artifacts (faster, keeps existing database)
- `-SkipBuild` - Skip build step (use if no code changes)
- `-OpenBrowser` - Automatically open browser to application URL
- `-Port <int>` - HTTP port to use (default: 5000)
- `-BrowserUrl <string>` - Custom URL to open in browser (default: http://localhost:5000)

**Examples:**
```powershell
# Full clean rebuild with browser
.\Launch\Relaunch.ps1 -OpenBrowser

# Keep database, rebuild, and open browser
.\Launch\Relaunch.ps1 -SkipClean -OpenBrowser

# Fast restart (skip clean and build)
.\Launch\Relaunch.ps1 -SkipClean -SkipBuild -OpenBrowser

# Launch on custom port
.\Launch\Relaunch.ps1 -Port 8080 -OpenBrowser

# Open to specific page
.\Launch\Relaunch.ps1 -OpenBrowser -BrowserUrl "http://localhost:5000/Collections"
```

---

### `QuickLaunch.ps1` (Fast Restart)
**Purpose:** Rapid restart without rebuilding - ideal for development iteration when you just need to restart the app

**Features:**
- Stops existing application processes
- Launches application without rebuilding
- Much faster than full relaunch (~2 seconds vs ~15 seconds)

**Parameters:**
- `-OpenBrowser` - Automatically open browser
- `-Port <int>` - HTTP port to use (default: 5000)
- `-BrowserUrl <string>` - Custom URL to open

**When to Use:**
- ✅ Restarting after configuration changes (`appsettings.json`)
- ✅ Clearing in-memory state (sessions, cache)
- ✅ Testing database seed data changes
- ❌ DO NOT use after code changes (C#, Razor) - requires full build

**Examples:**
```powershell
# Quick restart with browser
.\Launch\QuickLaunch.ps1 -OpenBrowser

# Quick restart on custom port
.\Launch\QuickLaunch.ps1 -Port 8080
```

---

### `ResetDatabase.ps1` (Database Reset)
**Purpose:** Delete the database file so it will be recreated with seed data on next launch

**Features:**
- Interactive confirmation (unless `-Force` used)
- Stops running application automatically
- Deletes database files (`.db`, `.db-shm`, `.db-wal`)
- Shows database size before deletion

**Parameters:**
- `-Force` - Skip confirmation prompt

**When to Use:**
- Want to reset to fresh seed data
- Database schema changed (after migrations)
- Testing seed data modifications
- Corrupted database

**Examples:**
```powershell
# Interactive reset (asks for confirmation)
.\Launch\ResetDatabase.ps1

# Force reset without confirmation
.\Launch\ResetDatabase.ps1 -Force
```

**Important:** After resetting, launch the application to recreate the database:
```powershell
.\Launch\ResetDatabase.ps1 -Force
.\Launch\Relaunch.ps1 -SkipBuild -OpenBrowser
```

---

## Common Workflows

### First Time Setup
```powershell
# Clean install with fresh database
.\Launch\Relaunch.ps1 -OpenBrowser
```

### Daily Development
```powershell
# After code changes
.\Launch\Relaunch.ps1 -SkipClean -OpenBrowser

# Quick restart (no code changes)
.\Launch\QuickLaunch.ps1 -OpenBrowser
```

### Database Development
```powershell
# Test new seed data
.\Launch\ResetDatabase.ps1 -Force
.\Launch\QuickLaunch.ps1 -OpenBrowser

# Apply new migration
dotnet ef database drop -f
dotnet ef database update
.\Launch\QuickLaunch.ps1 -OpenBrowser
```

### Troubleshooting
```powershell
# Complete fresh start (clears everything)
.\Launch\ResetDatabase.ps1 -Force
.\Launch\Relaunch.ps1 -OpenBrowser

# Check if ports are blocked
Get-NetTCPConnection -LocalPort 5000
Get-NetTCPConnection -LocalPort 5001

# Manually kill processes on ports
Get-NetTCPConnection -LocalPort 5000 | Select -Expand OwningProcess | ForEach-Object { Stop-Process -Id $_ -Force }
```

---

## Application URLs

After launching, the application is available at:

- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001

**Note:** HTTPS uses a development certificate. You may see browser warnings on first access.

---

## Port Configuration

Default ports:
- HTTP: 5000
- HTTPS: 5001

To use custom ports:
```powershell
.\Launch\Relaunch.ps1 -Port 8080 -OpenBrowser
# Available at: http://localhost:8080 and https://localhost:5001
```

**Note:** HTTPS port is always 5001 (not configurable via these scripts)

---

## Prerequisites

- **Windows:** Windows 10/11 or Windows Server 2019+
- **.NET SDK:** .NET 10.0 SDK or later
  - Download: https://dotnet.microsoft.com/download
  - Verify: `dotnet --version`
- **PowerShell:** PowerShell 5.1+ (included with Windows)
  - Verify: `$PSVersionTable.PSVersion`

---

## Archive Folder

The `Archive/` subfolder contains previous versions of scripts for reference:
- `Relaunch_backup.ps1` - Original verbose version with detailed console output

These are kept for historical reference and can be deleted if not needed.

---

## Troubleshooting

### "Cannot find .NET SDK"
Install .NET SDK from: https://dotnet.microsoft.com/download

### "Port already in use"
Run `Relaunch.ps1` which automatically stops processes on ports 5000/5001, or manually kill them:
```powershell
Get-NetTCPConnection -LocalPort 5000 | Select -Expand OwningProcess | Stop-Process -Force
```

### "Build failed"
Check the error messages for:
- **Syntax errors** - Fix C# compilation errors
- **Missing packages** - Run `dotnet restore`
- **Missing files** - Ensure all project files exist

### "QuickLaunch fails"
QuickLaunch requires a previous build. Run a full build first:
```powershell
.\Launch\Relaunch.ps1
```

### "Database locked"
Stop the application and try again:
```powershell
# Stop processes manually
Get-NetTCPConnection -LocalPort 5000 | Select -Expand OwningProcess | Stop-Process -Force

# Then reset database
.\Launch\ResetDatabase.ps1 -Force
```

---

## Tips & Best Practices

1. **Use `Launch.bat`** for non-technical users - it's the simplest entry point
2. **Use `QuickLaunch.ps1`** during active development for fast iteration
3. **Use `Relaunch.ps1 -SkipClean`** to preserve database during development
4. **Use `ResetDatabase.ps1`** when testing seed data or schema changes
5. **Always use `-OpenBrowser`** flag for convenience unless you already have browser open

---

## Script Execution Policy

If you get a "script execution disabled" error, run this once (as Administrator):
```powershell
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
```

Or use bypass mode for individual executions:
```powershell
powershell -ExecutionPolicy Bypass -File .\Launch\Relaunch.ps1
```
