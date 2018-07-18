namespace CaseWare.CommandLineParser
{
    public interface ICommandLine<out TArgs> where TArgs : ICommandArgs
    {
        TArgs Parse(string[] args);
    }
}