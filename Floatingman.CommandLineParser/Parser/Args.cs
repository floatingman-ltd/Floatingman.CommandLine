using System.Collections.Generic;

namespace Floatingman.CommandLineParser.Parser
{
    public class Args : List<string>, IArgs
    {
        public Args(string[] args)
        {
            AddRange(args);
        }
    }
}