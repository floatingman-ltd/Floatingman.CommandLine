using CaseWare.CommandLineParser.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using CaseWare.CommandLineParser;
using CaseWare.CommandLineParser.Parser;

namespace CaseWare.CommandLine.UsagePlugin
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
