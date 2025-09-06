using AssistMVP.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
namespace AssistMVP.Services {
  public class AiClient {
    private static readonly HttpClient Http = new HttpClient();
    public async Task<DetectionResult> AnalyzeAsync(BitmapSource frame){
      var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
      if(string.IsNullOrWhiteSpace(key)){
        return new DetectionResult{
          Issue="Sample: Wi‑Fi appears disconnected.",
          Steps=new List<StepInstruction>{
            new StepInstruction{Index=1,Instruction="Click the network icon in the taskbar.", AnchorText="Network & Internet"},
            new StepInstruction{Index=2,Instruction="Select your Wi‑Fi and click Connect."},
            new StepInstruction{Index=3,Instruction="Enter the password if prompted."}
          },
          Regions=new List<Region>{ new Region{X=1200,Y=1000,W=200,H=80} }
        };
      }
      var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";
      var png = EncodePng(frame);
      var dataUrl = "data:image/png;base64," + Convert.ToBase64String(png);
      var payload = new{
        model,
        messages = new object[]{
          new { role="system", content="You are a Windows tech support AI. From a screenshot, detect likely issues and return JSON: 'Issue', ordered 'Steps', optional 'Regions' to highlight UI areas. Keep it safe; never perform actions automatically." },
          new { role="user", content = new object[] {
              new { type="text", text="Analyze this screen. If there's a clear problem, propose 2-6 steps to fix it. JSON only." },
              new { type="image_url", image_url = new { url = dataUrl } }
          }}
        },
        temperature = 0.2,
        response_format = new { type = "json_object" }
      };
      using var req = new HttpRequestMessage(HttpMethod.Post,"https://api.openai.com/v1/chat/completions");
      req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
      req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
      using var resp = await Http.SendAsync(req);
      var json = await resp.Content.ReadAsStringAsync();
      if(!resp.IsSuccessStatusCode) throw new Exception($"OpenAI error {resp.StatusCode}: {json}");
      using var doc = JsonDocument.Parse(json);
      var content = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "{}";
      var result = JsonSerializer.Deserialize<DetectionResult>(content) ?? new DetectionResult();
      return result;
    }
    private static byte[] EncodePng(BitmapSource src){
      using var ms=new MemoryStream(); var enc=new PngBitmapEncoder(); enc.Frames.Add(BitmapFrame.Create(src)); enc.Save(ms); return ms.ToArray();
    }
  }
}
