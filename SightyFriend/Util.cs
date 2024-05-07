using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SightyFriend;

internal class Util
{
  public static List<(Vector3, Vector3)> GetAlignedSegments(Stack<Vector3> vectorStack)
  {
    var alignedSegments = new List<(Vector3, Vector3)>();

    if (vectorStack == null || vectorStack.Count < 2)
      return alignedSegments;

    var start = vectorStack.Pop();
    var end = vectorStack.Pop();

    while (vectorStack.Count > 0)
    {
      var next = vectorStack.Pop();

      if (IsAligned(start, end, next))
      {
        end = next;
      }
      else
      {
        alignedSegments.Add((start, end));
        start = end;
        end = next;
      }
    }

    alignedSegments.Add((start, end));

    return alignedSegments;
  }

  public static bool IsAligned(Vector3 start, Vector3 end, Vector3 next)
  {
    // Check if three points are collinear (aligned)
    return Math.Abs(((end.Y - start.Y) * (next.X - end.X)) - ((end.X - start.X) * (next.Y - end.Y))) < float.Epsilon;
  }
}
