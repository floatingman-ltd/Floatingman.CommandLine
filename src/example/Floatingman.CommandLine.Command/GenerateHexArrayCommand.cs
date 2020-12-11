using System;
using System.Collections.Generic;
using System.Text;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;
using Floatingman.Common.Extensions;
using static TopologyFunctions.GeometricMath;

namespace Floatingman.TopologyTools.GenerateHexArray
{

   public class GenerateHexArrayCommand : Command<GenerateHexArrayArgs>, ICommand
   {
      public override string Name => "GenerateHexArray";

      public override string ShortHelp => "Generates an array of hexes, Rows (U) * Columns (V) and Radius";

// this needs to emit a result event or we need to provide an overload that return an IAsyncEnum<string> ...
      public override async IAsyncEnumerable<string> Execute(GenerateHexArrayArgs args)
      {
         var sb = new StringBuilder();
         for (var u = 0; u < args.Columns; u++)
            for (var v = 0; v < args.Rows; v++)
            {
               var centroid = CalculateCentroid(u, v, args.Radius);
               yield return $"{centroid.X} {centroid.Y}";
            }
      }


   }
}
