namespace CaseWare.CommandLineParser
{
    internal class Token
    {
        public enum @Type
        {
            Option,
            Argument
        }

        public @Type type { get; set; }
    }
}