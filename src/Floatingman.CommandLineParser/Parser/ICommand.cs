namespace Floatingman.CommandLineParser.Parser
{
    public interface ICommand<in TArgs>
    {
        string Execute(TArgs args);
    }
}