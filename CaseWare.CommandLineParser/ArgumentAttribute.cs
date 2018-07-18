using System;

namespace CaseWare.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : AttributeBase
    {
        public ArgumentAttribute(int position) => Position = position;

        public int Position { get; }
    }
}