using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Floatingman.CommandLine.UsagePlugin;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;
using Floatingman.CommandLineParser.Plugin;

namespace Floatingman.CommandLine.UsageConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawArgs = new Args(args);
            var command = new UsageCommand();
            var commandLine = Floatingman.CommandLineParser.CommandLine.Instance;
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
