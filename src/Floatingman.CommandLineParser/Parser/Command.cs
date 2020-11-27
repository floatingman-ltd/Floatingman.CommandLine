namespace Floatingman.CommandLineParser.Parser
{
    public abstract class Command<TArgs> : ICommand<TArgs>
    {
        public string Execute(TArgs args)
        {
            return ExecuteCommand(args);
        }

        protected abstract string ExecuteCommand(TArgs args);
    }
}