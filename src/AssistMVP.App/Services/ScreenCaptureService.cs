using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
namespace AssistMVP.Services {
  public class ScreenCaptureService {
    public BitmapSource? CapturePrimaryScreen(){
      try{
        var bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
        using var bmp = new Bitmap(bounds.Width,bounds.Height,PixelFormat.Format32bppArgb);
        using(var g = Graphics.FromImage(bmp)){ g.CopyFromScreen(bounds.Left,bounds.Top,0,0,bounds.Size); }
        using var ms = new MemoryStream();
        bmp.Save(ms,ImageFormat.Png); ms.Position=0;
        var img = new BitmapImage(); img.BeginInit(); img.CacheOption=BitmapCacheOption.OnLoad; img.StreamSource=ms; img.EndInit(); img.Freeze();
        return img;
      }catch{ return null; }
    }
  }
}
