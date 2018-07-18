using System.Collections.Generic;

namespace CaseWare.CommandLineParser.Parser
{
    public class Args : List<string>, IArgs
    {
        public Args(string[] args)
        {
            AddRange(args);
        }
    }
}