using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AssistMVP.Services;
using AssistMVP.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AssistMVP.ViewModels {
  public partial class TroubleshooterViewModel : ObservableObject {
    private readonly ScreenCaptureService _capture = new();
    private readonly AiClient _ai = new();
    private readonly OverlayService _overlay = new();
    [ObservableProperty] private string captureButtonLabel = "Start Sharing";
    [ObservableProperty] private string info = "Ready";
    [ObservableProperty] private BitmapSource? lastCapture;
    [ObservableProperty] private DetectionResult detection = new();
    private CancellationTokenSource? _cts;
    [RelayCommand] private async Task ToggleCapture(){
      if(_cts!=null){ _cts.Cancel(); _cts=null; CaptureButtonLabel="Start Sharing"; Info="Stopped"; return; }
      _cts=new CancellationTokenSource(); CaptureButtonLabel="Stop Sharing"; Info="Capturing...";
      _ = Task.Run(async ()=>{
        while(!_cts!.IsCancellationRequested){
          var bmp=_capture.CapturePrimaryScreen(); if(bmp!=null) LastCapture=bmp;
          await Task.Delay(1000);
        }}, _cts.Token);
    }
    [RelayCommand] private void FlashOverlay(){ _overlay.Flash(new System.Windows.Rect(200,200,300,180)); }
    [RelayCommand] private async Task AnalyzeOnce(){
      if(LastCapture is null){ Info="No capture yet."; return; }
      Info="Analyzing...";
      try{
        Detection = await _ai.AnalyzeAsync(LastCapture);
        Info="Analysis complete.";
        if(Detection.Regions.Count>0) _overlay.Flash(Detection.Regions[0].ToRect());
      } catch(System.Exception ex){ Info="AI error: "+ex.Message; }
    }
  }
}
