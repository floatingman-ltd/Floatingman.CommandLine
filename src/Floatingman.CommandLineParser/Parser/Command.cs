
using System;
using System.Linq;
using Floatingman.Common.Extensions;

namespace Floatingman.CommandLineParser.Parser
{
   public abstract class Command<TParams> : ICommand<TParams> where TParams : ICommandArgs, new()
   {
      public abstract string Name { get; }

      public abstract string ShortHelp { get; }

      public abstract string Execute(TParams args);
      public string Execute(string[] args)
      {
         string[] fullArgs = null;
         var hasStream =  Console.IsInputRedirected;//.In.Peek() != -1;
         if (hasStream)
         {
            fullArgs = (args.Clone() as string[]).Concat(Console.In.ReadLine().Split(' ')).ToArray();
         } else {
            fullArgs = args.Clone() as string[];
         }
         // (new {fullArgs}).AsJson();
         TParams p = Parse(fullArgs);
         if (p.Errors.Count == 0)
         {
            return Execute(p);

         }
         else
         {
            return $"Input not in correct format:{System.Environment.NewLine + "\t" + string.Join(System.Environment.NewLine, p.Errors)}";
         }
      }

      public TParams Parse(string[] args)
      {
         return CommandLine.Instance.Parse<TParams>(args[1..^0]);
      }

   }
}
