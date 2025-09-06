using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AssistMVP.Services;
using Microsoft.Win32;
namespace AssistMVP.ViewModels {
  public partial class VPNViewModel : ObservableObject {
    private readonly VPNService _vpn=new();
    [ObservableProperty] private string info="Idle";
    [RelayCommand] private void Import(){ var dlg=new OpenFileDialog{Filter="WireGuard Config (*.conf)|*.conf"}; if(dlg.ShowDialog()==true){ if(_vpn.ImportConfig(dlg.FileName)) Info="Config imported (placeholder)."; } }
    [RelayCommand] private void Start(){ Info=_vpn.Start()? "Started (placeholder)" : "Failed to start"; }
    [RelayCommand] private void Stop(){ Info=_vpn.Stop()? "Stopped (placeholder)" : "Failed to stop"; }
  }
}
