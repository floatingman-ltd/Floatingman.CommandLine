using System;
using System.Collections.Generic;
using System.Text;
using CaseWare.CommandLineParser;
using CaseWare.CommandLineParser.Parser;

namespace CaseWare.CommandLine.UsagePlugin
{
    public class UsageCommand:ICommand<UsageArgs>
    {
        public string Execute(UsageArgs args)
        {
            return "run, run away";
        }
    }
}
