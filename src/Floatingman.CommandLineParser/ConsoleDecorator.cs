using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

using Floatingman.CommandLineParser.Parser;
using System.IO;
using System.Linq;

namespace Floatingman.CommandLineParser
{
   public abstract class ConsoleDecorator
   {

      static ConsoleDecorator()
      {
         // find the plugins - the current usage will hard code the expected file name pattern as *.Plugin.dll
      }

      public static void Run(string[] args)
      {
         try
         {
            // this list needs to come from the a configuration file
            // https://docs.microsoft.com/en-us/dotnet/core/extensions/options
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
               var command = commands.FirstOrDefault(c => c.Name == args[0]);
               // if (command == null)
               // {
               //     Console.WriteLine("No such command is known.");
               //     return;
               // }

               Console.WriteLine(command.Execute(args));
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex);
         }
      }

      private static object DiscoverPlugins()
      {
         throw new System.NotImplementedException();
      }

      protected static Assembly LoadPlugin(string relativePath)
      {
         // Navigate up to the solution root
         string root = Path.GetFullPath(Path.Combine(
             Path.GetDirectoryName(
                 Path.GetDirectoryName(
                     Path.GetDirectoryName(
                         Path.GetDirectoryName(
                             Path.GetDirectoryName(typeof(ConsoleDecorator).Assembly.Location)))))));

         string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
         Console.WriteLine($"Loading commands from: {pluginLocation}");
         PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
         return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
      }

      protected static IEnumerable<ICommand> CreateCommands(Assembly assembly)
      {
         int count = 0;

         foreach (Type type in assembly.GetTypes())
         {
            // if (type.GetInterface("ICommand`1",true) != null)
            if (typeof(ICommand).IsAssignableFrom(type))
            {
               Console.WriteLine(type.Name);
               // var result = type.GetMethod("Factory",BindingFlags.Public|BindingFlags.Static).Invoke(null,null);
               var result = Activator.CreateInstance(type) as ICommand;
               if (result != null)
               {
                  count++;
                  yield return result;
               }
            }
         }

         if (count == 0)
         {
            string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
            throw new ApplicationException(
                $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                $"Available types: {availableTypes}");
         }
      }
   }

}
