namespace CaseWare.CommandLineParser.Parser
{
    public abstract class Command<TArgs> : ICommand<TArgs>
    {
        protected string History { get; set; }

        public string Execute(TArgs args)
        {
            History = args.ToString();

            return ExecuteCommand(args);
        }

        protected abstract string ExecuteCommand(TArgs args);
    }
}