using System;
using System.Collections.Generic;
using System.Text;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;

namespace Floatingman.CommandLine.UsagePlugin
{
    public class UsageCommand:ICommand<UsageArgs>
    {
        public string Execute(UsageArgs args)
        {
            return "run, run away";
        }
    }
}
