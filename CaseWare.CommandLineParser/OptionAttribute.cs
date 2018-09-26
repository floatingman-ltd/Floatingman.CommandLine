using System;

namespace CaseWare.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : AttributeBase
    {
        public OptionAttribute(char shortForm) => ShortForm = shortForm;

        public OptionAttribute(string longForm) => LongForm = longForm;

        public OptionAttribute(char shortForm, string longForm) : this(shortForm) => LongForm = longForm;

        public string LongForm { get; }
        public char ShortForm { get; }
        public bool AllowMultiple { get; set; } = false;
    }
}