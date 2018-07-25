using System.Linq;
using CaseWare.CommandLineParser.Parser;

namespace CaseWare.CommandLineParser.Plugin
{
    public abstract class Plugin<TArgs> : IPlugin<TArgs> where TArgs : ICommandArgs, new()
    {
        private readonly ICommand<TArgs> _command;
        private readonly ICommandLine _commandLine;
        private readonly string[] _rawArgs;

        protected Plugin(IArgs rawArgs, ICommand<TArgs> command, ICommandLine commandLine)
        {
            _rawArgs = rawArgs.ToArray();
            _command = command;
            _commandLine = commandLine;
        }

        public TArgs Args { get; set; }
        public string Name { get; } = "usage";

        public string Execute()
        {
            Args = _commandLine.Parse<TArgs>(_rawArgs);
            return _command.Execute(Args);
        }

        public abstract string Execute(string args);

        public string Execute(TArgs args)
        {
            return _command.Execute(args);
        }
    }
}