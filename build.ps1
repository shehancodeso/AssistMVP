# One-click local build for Windows (no Visual Studio required)
$ErrorActionPreference = "Stop"
$DotnetVersion = "8.0.401"
$Root = (Get-Location).Path
$DotnetRoot = Join-Path $Root ".dotnet"
$Dist = Join-Path $Root "dist"
New-Item -ItemType Directory -Force -Path $DotnetRoot | Out-Null
New-Item -ItemType Directory -Force -Path $Dist | Out-Null
$Installer = Join-Path $env:TEMP "dotnet-install.ps1"
Invoke-WebRequest -UseBasicParsing -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile $Installer
& powershell -ExecutionPolicy Bypass -File $Installer -Version $DotnetVersion -InstallDir $DotnetRoot
$env:DOTNET_ROOT = $DotnetRoot
$env:PATH = "$DotnetRoot;$env:PATH"
& "$DotnetRoot\dotnet.exe" --info
$proj = ".\src\AssistMVP.App\AssistMVP.App.csproj"
& "$DotnetRoot\dotnet.exe" restore $proj
& "$DotnetRoot\dotnet.exe" publish $proj -c Release -r win-x64 --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:EnableWindowsTargeting=true `
  -o $Dist
Write-Host "✅ Build complete → dist\AssistMVP.exe" -ForegroundColor Green
