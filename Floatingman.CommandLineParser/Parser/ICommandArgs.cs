using System.Collections.Generic;

namespace Floatingman.CommandLineParser
{
    public interface ICommandArgs
    {
        string Command { get; set; }
        List<string> Errors { get; }
    }
}