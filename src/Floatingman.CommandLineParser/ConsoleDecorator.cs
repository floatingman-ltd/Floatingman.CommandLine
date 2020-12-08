using System;
using System.Threading.Tasks;
using Floatingman.CommandLineParser.Parser;

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
      public static Command<TParams> LoadCommand<TParams>(string commandName)where TParams : CommandArgs, new()
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
   }

}
