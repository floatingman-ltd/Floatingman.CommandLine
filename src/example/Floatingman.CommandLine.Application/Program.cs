using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;

namespace AppWithPlugin
{
    class Program:ConsoleDecorator
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && args[0] == "/d")
                {
                    Console.WriteLine("Waiting for any key...");
                    Console.ReadLine();
                }

// this list needs to come from the a configuration file
                string[] pluginPaths = new string[]
                {
                    @"Floatingman.CommandLine.Command\bin\Debug\net5.0\\Floatingman.CommandLine.Command.dll"
                };

                var commands = pluginPaths.SelectMany(pluginPath =>
                {
                    Assembly pluginAssembly = LoadPlugin(pluginPath);
                    return CreateCommands(pluginAssembly);
                }).ToList();

                if (args.Length == 0)
                {
                    Console.WriteLine("Commands: ");
                    foreach (var command in commands)
                    {
                        Console.WriteLine($"{command.Name}\t - {command.ShortHelp}");
                    }
                }
                else
                {
                    // foreach (string commandName in args)
                    // {
                        // Console.WriteLine($"-- {commandName} --");
                        var command = commands.FirstOrDefault(c => c.Name == args[0]);
                        // if (command == null)
                        // {
                        //     Console.WriteLine("No such command is known.");
                        //     return;
                        // }

                        Console.WriteLine( command.Execute(args));
                        // Console.WriteLine();
                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
