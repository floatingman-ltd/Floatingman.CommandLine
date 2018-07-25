using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CaseWare.CommandLine.UsagePlugin;
using CaseWare.CommandLineParser;
using CaseWare.CommandLineParser.Parser;
using CaseWare.CommandLineParser.Plugin;

namespace CaseWare.CommandLine.UsageConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawArgs = new Args(args);
            var command = new UsageCommand();
            var commandLine = CaseWare.CommandLineParser.CommandLine.Instance;
            var plugin = new UsagePlugin.UsagePlugin(rawArgs, command, commandLine);

            // The plugins that use the IoC container end up creating an instance of each plugin to determine the name.
            // 1. get a list of the plugins
            var plugins = new PluginCollection
            {
                plugin
            };

            // 2. get the Plugin "verb" from the command-line
            IPlugin verb = plugins.GetPluginCommand(rawArgs);

            //var verb = CommandLine.GetVerb(rawArgs);

            // 3. execute the plugin
            var result = verb.Execute();

            Console.WriteLine(result);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
