@echo off
setlocal
powershell -ExecutionPolicy Bypass -File "%~dp0build.ps1"
if %ERRORLEVEL% neq 0 ( echo Build failed. & exit /b %ERRORLEVEL% )
echo.
echo ✅ Build complete. EXE is in .\dist\AssistMVP.exe
