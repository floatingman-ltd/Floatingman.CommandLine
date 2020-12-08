using System;
using System.Text;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;

using static TopologyFunctions.GeometricMath;

namespace Floatingman.TopologyTools.GenerateHexArray
{

    // public class GenerateHexArrayCommand : ICommand<ICommandArgs>
    public class GenerateHexArrayCommand : Command<GenerateHexArrayArgs>, ICommand
    {
        public override string Name =>  "GenerateHexArray";

        public override string ShortHelp => "Generates an array of hexes, Rows (U) * Columns (V) and Radius";

        public string Description => ShortHelp;

        public override string Execute(GenerateHexArrayArgs args)
        // public string Execute(ICommandArgs args)
        {
            var x = args as GenerateHexArrayArgs;
            var sb = new StringBuilder();
            for (var u = 0; u < x.Columns; u++)
                for (var v = 0; v < x.Rows; v++)
                {
                    var centroid = CalculateCentroid(u, v, x.Radius);
                    sb.Append($"POINT ({centroid.X} {centroid.Y}){Environment.NewLine}");
                }
            return sb.ToString();
        }


    }
}
