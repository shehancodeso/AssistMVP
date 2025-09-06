using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
namespace AssistMVP.Services {
  public class OverlayService {
    private OverlayWindow? _window;
    public void Flash(Rect rect){
      Application.Current.Dispatcher.Invoke(()=>{
        _window ??= new OverlayWindow(); _window.Show(); _window.Flash(rect);
      });
    }
    private class OverlayWindow : Window {
      private readonly Rectangle _rect = new Rectangle{ Stroke=Brushes.LimeGreen, StrokeThickness=4, RadiusX=8, RadiusY=8 };
      private readonly System.Windows.Controls.Canvas _canvas = new();
      public OverlayWindow(){
        AllowsTransparency=true; Background=Brushes.Transparent; WindowStyle=WindowStyle.None; Topmost=true; ShowInTaskbar=false; IsHitTestVisible=false;
        Width=SystemParameters.PrimaryScreenWidth; Height=SystemParameters.PrimaryScreenHeight; Left=0; Top=0; Content=_canvas;
        var hwnd=new System.Windows.Interop.WindowInteropHelper(this).EnsureHandle();
        int exStyle=GetWindowLong(hwnd,GWL_EXSTYLE); SetWindowLong(hwnd,GWL_EXSTYLE, exStyle|WS_EX_LAYERED|WS_EX_TRANSPARENT);
      }
      public void Flash(Rect r){
        _canvas.Children.Clear(); System.Windows.Controls.Canvas.SetLeft(_rect,r.X); System.Windows.Controls.Canvas.SetTop(_rect,r.Y); _rect.Width=r.Width; _rect.Height=r.Height; _canvas.Children.Add(_rect);
        var a=new System.Windows.Media.Animation.DoubleAnimation{ From=1.0, To=0.0, Duration=TimeSpan.FromSeconds(1.2), AutoReverse=false };
        a.Completed += (_,__) => { _canvas.Children.Clear(); }; _rect.BeginAnimation(UIElement.OpacityProperty, a);
      }
      private const int GWL_EXSTYLE=-20; private const int WS_EX_TRANSPARENT=0x20; private const int WS_EX_LAYERED=0x80000;
      [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hWnd,int nIndex);
      [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd,int nIndex,int dwNewLong);
    }
  }
}
