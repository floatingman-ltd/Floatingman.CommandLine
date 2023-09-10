using System.Collections.Generic;

namespace Topology.Records{
  public record Polygon (IEnumerable<LineString> lineStrings);
}
