using System;

namespace Floatingman.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : AttributeBase
    {
        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}