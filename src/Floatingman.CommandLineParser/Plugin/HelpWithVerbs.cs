// using System.Collections.Generic;
// using Floatingman.CommandLineParser.Parser;

// namespace Floatingman.CommandLineParser.Plugin
// {
//     public class HelpWithVerbs : Plugin<HelpArgs>
//     {
//         private readonly IEnumerable<IPlugin> _verbs;


//         public HelpWithVerbs(IArgs rawArgs, ICommand<HelpArgs> command, ICommandLine<HelpArgs> commandLine) : base(rawArgs, command, commandLine)
//         {
//         }

//         public override string Name { get; internal set; } = "help";


//         public override string Execute(string args)
//         {
//             throw new System.NotImplementedException();
//         }
//     }
// }