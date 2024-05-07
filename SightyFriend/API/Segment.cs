using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Plugin.Services;

namespace SightyFriend.API;

public class WalkableSegment
{
  public int x1 { get; set; }
  public int y1 { get; set; }
  public int z1 { get; set; }
  public int x2 { get; set; }
  public int y2 { get; set; }
  public int z2 { get; set; }
  public string map { get; set; }
  public Vector3 EndPoint
  {
    get
    {
      return new Vector3(x2, y2, z2);
    }
    set
    {
      x2 = (int)Math.Round(value.X);
      y2 = (int)Math.Round(value.Y);
      z2 = (int)Math.Round(value.Z);
    }
  }
  public Vector3 FirstPoint
  {
    get
    {
      return new Vector3(x2, y2, z2);
    }
    set
    {
      x2 = (int)Math.Round(value.X);
      y2 = (int)Math.Round(value.Y);
      z2 = (int)Math.Round(value.Z);
    }
  }
}
