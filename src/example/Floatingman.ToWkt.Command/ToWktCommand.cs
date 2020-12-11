using System;
using System.Text;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;
using Floatingman.Common.Extensions;
using static TopologyFunctions.GeometricMath;

namespace Floatingman.TopologyTools
{

    public class ToWktCommand : Command<ToWktArgs>, ICommand
    {
        public override string Name =>  "ToWkt";

        public override string ShortHelp => "Converts a X Y Corrdinate into a WKT Point";

        public override string Execute(ToWktArgs args)
        {
            return $"POINT ( {args.X} {args.Y} )";
        }


    }
}
