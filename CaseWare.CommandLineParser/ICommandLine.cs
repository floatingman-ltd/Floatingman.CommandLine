namespace CaseWare.CommandLineParser
{
    public interface ICommandLine
    {
        TArgs Parse<TArgs>(string[] args) where TArgs : ICommandArgs,new();
    }
}