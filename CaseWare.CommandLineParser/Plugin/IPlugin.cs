namespace CaseWare.CommandLineParser.Plugin
{
    public interface IPlugin<TArgs> : IPlugin
    {
        TArgs Args { get; set; }
    }

    public interface IPlugin
    {
        string Name { get; set; }

        string Execute();

        string Execute(string args);
    }
}