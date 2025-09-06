using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AssistMVP.Models;

namespace AssistMVP.Services {
  public class OptimizerService {
    public async Task<long> CleanTempAsync(){
      long total=0; total+=await DeleteFilesInFolderAsync(Path.GetTempPath());
      var winTemp=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),"Temp");
      total+=await DeleteFilesInFolderAsync(winTemp); return total;
    }
    private Task<long> DeleteFilesInFolderAsync(string path){
      return Task.Run(()=>{
        long bytes=0;
        try{
          if(!Directory.Exists(path)) return 0;
          foreach(var f in Directory.EnumerateFiles(path,"*",SearchOption.AllDirectories)){
            try{ var fi=new FileInfo(f); bytes+=fi.Length; fi.IsReadOnly=false; File.Delete(f);}catch{}
          }
        }catch{}
        return bytes;
      });
    }
  }

  public class StartupManager {
    private const string RunKey=@"Software\Microsoft\Windows\CurrentVersion\Run";
    public IEnumerable<StartupItem> ListUserRun(){
      using var key=Registry.CurrentUser.OpenSubKey(RunKey,false); if(key==null) yield break;
      foreach(var name in key.GetValueNames()) yield return new StartupItem{ Name=name, Command=(key.GetValue(name)?.ToString() ?? "") };
    }
    public bool RemoveUserRun(string name){
      try{ using var key=Registry.CurrentUser.OpenSubKey(RunKey,true); if(key==null) return false; key.DeleteValue(name,false); return true; }catch{ return false; }
    }
  }

  public class ProcessManager {
    public Task<List<ProcInfo>> GetHeavyAsync(int topN){
      return Task.Run(()=>{
        var list=new List<ProcInfo>();
        foreach(var p in Process.GetProcesses()){
          try{ list.Add(new ProcInfo{ Name=p.ProcessName, Pid=p.Id, MemoryMB=p.WorkingSet64/1024.0/1024.0 }); }catch{}
        }
        return list.OrderByDescending(x=>x.MemoryMB).Take(topN).ToList();
      });
    }
    public bool TryKill(int pid){
      try{ var p=Process.GetProcessById(pid); p.Kill(entireProcessTree:true); return true; }catch{ return false; }
    }
  }
}
