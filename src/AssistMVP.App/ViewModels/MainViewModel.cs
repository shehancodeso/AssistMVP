using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AssistMVP.Views;
namespace AssistMVP.ViewModels {
  public partial class MainViewModel : ObservableObject {
    [ObservableProperty] private object? currentView;
    [ObservableProperty] private string status = "Idle";
    public MainViewModel(){ CurrentView = new TroubleshooterView(); }
    [RelayCommand] void ShowTroubleshooter()=> CurrentView=new TroubleshooterView();
    [RelayCommand] void ShowOptimizer()=> CurrentView=new OptimizerView();
    [RelayCommand] void ShowVpn()=> CurrentView=new VPNView();
  }
}
