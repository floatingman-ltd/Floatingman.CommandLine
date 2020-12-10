using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

using Floatingman.CommandLineParser.Parser;
using System.IO;
using System.Linq;
using System.IO.Abstractions;
using Floatingman.Common.Extensions;

namespace Floatingman.CommandLineParser
{
   public abstract class ConsoleDecorator
   {

      static ConsoleDecorator()
      {
      }

      public static void Run(string[] args)
      {
         try
         {

            var fileSystem = new FileSystem();
            // var cwd = fileSystem.Directory..GetCurrentDirectory();
            // System.Reflection.Assembly.GetEntryAssembly().Location.AsJson();
            // this list needs to come from the a configuration file
            // https://docs.microsoft.com/en-us/dotnet/core/extensions/options
            // find the plugins - the current usage will hard code the expected file name pattern as ./plugins/*.Command.dll
            var pluginPaths = fileSystem.Directory.EnumerateFiles("plugins", "*.dll", SearchOption.AllDirectories).Where(f => f.Contains(".Command."));
            var commands = pluginPaths.SelectMany(pluginPath =>
            {
               Assembly pluginAssembly = LoadPlugin(pluginPath, fileSystem);
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
               if (command == null)
               {
                  Console.WriteLine("No such command is known.");
                  return;
               }
               Console.WriteLine(command.Execute(args));
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex);
         }
      }

      protected static Assembly LoadPlugin(string relativePath, FileSystem fileSystem)
      {
         PluginLoadContext loadContext = new PluginLoadContext(relativePath);
         return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(relativePath)));
      }

      protected static IEnumerable<ICommand> CreateCommands(Assembly assembly)
      {
         int count = 0;

         foreach (Type type in assembly.GetTypes())
         {
            if (typeof(ICommand).IsAssignableFrom(type))
            {
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
