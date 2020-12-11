using System;
using System.Collections.Generic;
using Floatingman.CommandLineParser;

namespace Floatingman.TopologyTools
{

   public class ToWktArgs : ICommandArgs
   {
      // X coordinate
      [Argument(0,IsRequired=true,Name="X")]
      public double X { get; set; }

      // Y coordinate
      [Argument(1,IsRequired=true,Name="Y")]
      public double Y { get; set; }

      public string Command { get; set; } = "ToWkt";

      public List<string> Errors { get; } = new List<string>();
   }
}
