using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AssistMVP.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace AssistMVP.ViewModels {
  public partial class OptimizerViewModel : ObservableObject {
    private readonly OptimizerService _opt=new();
    private readonly ProcessManager _proc=new();
    private readonly StartupManager _startup=new();
    [ObservableProperty] private string info="Ready";
    public ObservableCollection<ProcInfo> HeavyProcesses{get;}=new();
    public ObservableCollection<StartupItem> StartupItems{get;}=new();
    [RelayCommand] private async Task CleanTemp(){ var bytes=await _opt.CleanTempAsync(); Info=$"Deleted ~{bytes/(1024*1024)} MB of temp files."; }
    [RelayCommand] private async Task Boost(){ var bytes=await _opt.CleanTempAsync(); await RefreshStartup(); Info=$"Boosted. Cleaned ~{bytes/(1024*1024)} MB."; }
    [RelayCommand] private async Task RefreshStartup(){
      StartupItems.Clear(); foreach(var s in _startup.ListUserRun()) StartupItems.Add(s);
      HeavyProcesses.Clear(); foreach(var p in await _proc.GetHeavyAsync(10)) HeavyProcesses.Add(p);
    }
    [RelayCommand] private void KillProcess(int pid)=>_proc.TryKill(pid);
    [RelayCommand] private void RemoveStartup(string name){ if(_startup.RemoveUserRun(name)) Info=$"Removed '{name}' from startup."; }
  }
}
