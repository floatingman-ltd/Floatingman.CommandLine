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

      public ICommandArgs Parameters { get; set; }

      static ConsoleDecorator()
      {
         // find the plugins - the current usage will hard code the expected file name pattern as *.Plugin.dll
         var plugins = DiscoverPlugins();
      }

      public static void Run(string[] args)
      {
         // var command = LoadCommand(args[0]);
         // var parameters = Parse(command, args);
         // Execute(command, parameters);
      }

      // private static ICommandArgs Parse(Command command, string[] args)
      // {
      //    return command.Parse(args);
      // }
      // public static string Execute(Command command, ICommandArgs parameters)
      // {
      //    return command.Execute(parameters);
      // }

      private static TParams Parse<TParams>(string[] args) where TParams : CommandArgs, new()
      {
         // parse the command name from the args 
         var commandName = args[0];

         return CommandLine.Instance.Parse<TParams>(args);
      }

      // public static Command<TParams> LoadCommand<TParams>(string commandName) where TParams: CommandArgs, new()
      public static Command<TParams> LoadCommand<TParams>(string commandName) where TParams : CommandArgs, new()
      {
         throw new NotImplementedException(nameof(LoadCommand));
         // get the plugin and load it
         // return LoadPlugin(commandName);
      }

      // private static void Execute<TParams>(TParams parameters) where TParams : ICommandArgs
      // {
      //    _command.Execute(parameters);
      // }
      //    // parse out the commands
      //    var parameters = command.Parse(args);

      //    // execute
      //    command.ExecuteCommand(parameters);
      // }

      private static object DiscoverPlugins()
      {
         throw new System.NotImplementedException();
      }

      // private static Command LoadPlugin(string commandName)
      // {
      //    throw new System.NotImplementedException();
      // }
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
