using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SightyFriend.API;
public class Map
{
  public int id { get; set; }
  public List<WalkableSegment> walkableSegments { get; set; }
  public string placeName { get; set; }
}
