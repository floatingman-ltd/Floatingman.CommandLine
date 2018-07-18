using System;

namespace CaseWare.CommandLineParser
{
    public class AttributeBase : Attribute
    {
        public object Default { get; set; }
        public string Help { get; set; }
        public bool IsRequired { get; set; } = false;
        public string Name { get; set; }
        internal bool IsSet { get; set; }
    }
}