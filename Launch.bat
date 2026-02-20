@echo off
REM Firmware Feature Management - One-Click Launcher
REM This script provides easy access to the application launch scripts

setlocal
set SCRIPT_DIR=%~dp0

echo ================================================
echo    Firmware Feature Management - Launcher
echo ================================================
echo.

REM Check if PowerShell is available
where powershell >nul 2>nul
if errorlevel 1 (
    echo ERROR: PowerShell is not available on this system
    echo Please install PowerShell to use this launcher
    pause
    exit /b 1
)

REM Check if .NET SDK is installed
dotnet --version >nul 2>nul
if errorlevel 1 (
    echo ERROR: .NET SDK is not installed
    echo.
    echo Please download and install .NET SDK from:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

REM Launch the application using PowerShell script
echo Starting application with auto-browser launch...
echo.
powershell -ExecutionPolicy Bypass -File "%SCRIPT_DIR%Launch\Relaunch.ps1" -OpenBrowser

REM If the script exits, pause to show any errors
if errorlevel 1 (
    echo.
    echo ================================================
    echo    Launch failed - see errors above
    echo ================================================
    pause
    exit /b 1
)

endlocal
