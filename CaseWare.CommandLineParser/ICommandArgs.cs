using System.Collections.Generic;

namespace CaseWare.CommandLineParser
{
    public interface ICommandArgs
    {
        string Command { get; set; }
        List<string> Errors { get; }
    }
}