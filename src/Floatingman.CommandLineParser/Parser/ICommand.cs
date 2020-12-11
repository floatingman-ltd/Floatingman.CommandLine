namespace Floatingman.CommandLineParser.Parser
{
   public interface ICommand
   {
      string Name { get; }
      string ShortHelp { get; }

      string Execute(string[] args);
   }

   public interface ICommand<TParams> : ICommand where TParams : ICommandArgs, new()
   {

      string Execute(TParams args);
      TParams Parse(string[] args);
   }

}