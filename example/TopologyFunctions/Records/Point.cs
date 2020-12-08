using System;

using static System.Math;

namespace Topology.Records
{

  [Serializable]
  public class Point
  {
    public Point(double x, double y)
    {
      _x = x;
      _y = y;
    }
    private double _x;
    private double _y;

    public double X { get => Round(_x, 2); private set => _x = value; }
    public double Y { get => Round(_y, 2); private set => _y = value; }
    public static Point operator +(Point origin, Point offset) => new Point(origin.X + offset.X, origin.Y + offset.Y);

  }
}