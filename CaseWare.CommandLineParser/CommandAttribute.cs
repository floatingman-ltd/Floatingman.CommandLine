using System;

namespace CaseWare.CommandLineParser
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