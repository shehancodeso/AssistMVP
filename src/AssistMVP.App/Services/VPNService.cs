using System.Diagnostics;
using System.IO;
namespace AssistMVP.Services {
  public class VPNService {
    private string? _configPath;
    public bool ImportConfig(string filePath){ if(!File.Exists(filePath)) return false; _configPath=filePath; return true; }
    public bool Start(){
      if(_configPath is null) return false;
      try{ var psi=new ProcessStartInfo{ FileName="wireguard.exe", Arguments=$"/installtunnelservice "{_configPath}"", UseShellExecute=true, Verb="runas" }; Process.Start(psi); return true; }catch{ return false; }
    }
    public bool Stop(){ try{ var psi=new ProcessStartInfo{ FileName="wireguard.exe", Arguments="/stopall", UseShellExecute=true, Verb="runas" }; Process.Start(psi); return true; }catch{ return false; } }
  }
}
