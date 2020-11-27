using Floatingman.CommandLineParser.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;

namespace Floatingman.CommandLine.UsagePlugin
{
    public class UsagePlugin:Plugin<UsageArgs>
    {
        public UsagePlugin(IArgs rawArgs, ICommand<UsageArgs> command, ICommandLine commandLine) : base(rawArgs, command, commandLine)
        {
        }

        public override string Execute(string args)
        {
            throw new NotImplementedException();
        }
    }
}
