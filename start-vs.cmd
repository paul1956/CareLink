@echo off
setlocal enabledelayedexpansion

set VSDebugger_ValidateDotnetDebugLibSignatures=0

:: This command launches a Visual Studio solution with environment variables required to use a local version of the .NET Core SDK.

:: This tells .NET Core not to go looking for .NET Core in other places
set DOTNET_MULTILEVEL_LOOKUP=0

:: Put our local dotnet.exe on PATH first so Visual Studio knows which one to use
set PATH=%DOTNET_ROOT%;%PATH%

:: Prefer the VS in the developer command prompt if we're in one, followed by whatever shows up in the current search path.
set "DEVENV=%DevEnvDir%devenv.exe"

if exist "%DEVENV%" (
    :: Fully qualified works
    set "COMMAND=start "" /B "%ComSpec%" /S /C ""%DEVENV%" "%~dp0CareLink.slnx"""
) else (
    where devenv.exe /Q
    if !errorlevel! equ 0 (
        :: On the PATH, use that.
        set "COMMAND=start "" /B "%ComSpec%" /S /C "devenv.exe "%~dp0CareLink.slnx"""
    ) else (
        :: Can't find devenv.exe, let file associations take care of it
        set "COMMAND=start /B .\CareLink.slnx"
    )
)

%COMMAND%
