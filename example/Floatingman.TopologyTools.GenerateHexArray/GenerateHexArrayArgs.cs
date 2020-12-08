using System;
using System.Collections.Generic;
using Floatingman.CommandLineParser;

namespace Floatingman.TopologyTools.GenerateHexArray
{

    public class GenerateHexArrayArgs : ICommandArgs
    {
        // number of rows to generate
        [Option('U', "Rows")]
        public int Rows { get; set; }

        // number of columns to generate
        [Option('V', "Columns")]
        public int Columns { get; set; }

        // radius of the vertices of the hex
        [Option('r', "Radius")]
        public double Radius { get; set; }

        public string Command { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<string> Errors => new List<string>();
    }
}
