using System.Collections.Generic;
using System.Windows;
namespace AssistMVP.Models {
  public class DetectionResult { public string Issue{get;set;}="No obvious issue detected."; public List<StepInstruction> Steps{get;set;}=new(); public List<Region> Regions{get;set;}=new(); }
  public class StepInstruction { public int Index{get;set;} public string Instruction{get;set;}="" ; public string? AnchorText{get;set;} }
  public class Region { public double X{get;set;} public double Y{get;set;} public double W{get;set;} public double H{get;set;} public Rect ToRect()=>new Rect(X,Y,W,H); }
  public class StartupItem { public string Name{get;set;}="" ; public string Command{get;set;}="" ; }
  public class ProcInfo { public string Name{get;set;}="" ; public int Pid{get;set;} public double MemoryMB{get;set;} }
}
