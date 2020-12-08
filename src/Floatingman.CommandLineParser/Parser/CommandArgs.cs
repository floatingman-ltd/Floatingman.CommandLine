using System.Collections.Generic;

namespace Floatingman.CommandLineParser
{
    public class CommandArgs : ICommandArgs
    {
        public string Command { get; set; }

        // yes i know, I have a concrete type, this needs to use List.AddRange downstream
        public List<string> Errors { get; } = new List<string>();

        [Option('v', "verbose", Help = "Be verbose.")]
        public bool Verbose { get; set; }
    }
}