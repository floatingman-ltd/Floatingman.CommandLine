using System.Collections;
using System.Collections.Generic;

namespace Topology.Records
{
  public record LineString(IEnumerable<Point> points) : IEnumerable<Point>
  {
    public IEnumerator<Point> GetEnumerator()
    {
      return points.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)points).GetEnumerator();
    }
  }
}
