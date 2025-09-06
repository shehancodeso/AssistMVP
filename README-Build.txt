AssistMVP — One‑Click Build (No Visual Studio)

1) Extract this ZIP (e.g., C:\AssistMVP\)
2) Double‑click build.cmd  (or run build.ps1)
   - Installs a local .NET SDK (no admin needed)
   - Publishes a self‑contained single‑file EXE for win‑x64
3) Launch: dist\AssistMVP.exe

AI (optional):
  setx OPENAI_API_KEY "sk-..."
  setx OPENAI_MODEL "gpt-4o-mini"

If PowerShell is blocked:
  Start PowerShell and run:
    Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Bypass
