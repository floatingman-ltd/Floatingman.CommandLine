using System.Collections.Generic;

namespace Floatingman.CommandLineParser.Parser
{
   public interface ICommand
   {
      string Name { get; }
      string ShortHelp { get; }

      IAsyncEnumerable<string> Execute(string[] args);
   }

   public interface ICommand<TParams> : ICommand where TParams : ICommandArgs, new()
   {

      IAsyncEnumerable<string> Execute(TParams args);
      TParams Parse(string[] args);
   }

}
