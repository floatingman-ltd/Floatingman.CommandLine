
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

   public abstract class Command<TParams> : ICommand<TParams> where TParams : ICommandArgs, new()
   {
      public abstract string Name { get; }

      public abstract string ShortHelp { get; }

      public abstract string Execute(TParams args);
      public string Execute(string[] args)
      {
         TParams p = Parse(args);
         return Execute(p);
      }

      public TParams Parse(string[] args)
      {
         return CommandLine.Instance.Parse<TParams>(args[1..^0]);
      }

   }
}
