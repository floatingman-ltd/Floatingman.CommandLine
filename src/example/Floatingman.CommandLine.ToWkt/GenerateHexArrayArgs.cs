using System;
using System.Collections.Generic;
using Floatingman.CommandLineParser;

namespace Floatingman.TopologyTools.GenerateHexArray
{

   public class GenerateHexArrayArgs : ICommandArgs
   {
      // number of rows to generate
      [Option('U', "rows")]
      public int Rows { get; set; }

      // number of columns to generate
      [Option('V', "columns")]
      public int Columns { get; set; }

      // radius of the vertices of the hex
      [Option('R', "radius")]
      public double Radius { get; set; }

      public string Command { get; set; } = "GenerateHexArray";

      public List<string> Errors { get; } = new List<string>();
   }
}
