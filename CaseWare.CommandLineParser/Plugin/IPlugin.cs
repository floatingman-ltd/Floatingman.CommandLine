namespace CaseWare.CommandLineParser.Plugin
{
    public interface IPlugin<TArgs> : IPlugin
    {
        TArgs Args { get; set; }
    }

    public interface IPlugin
    {

        string Execute();

        string Execute(string args);

        string Name { get; }
    }
}