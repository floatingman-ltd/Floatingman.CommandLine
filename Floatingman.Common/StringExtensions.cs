using System.Text.RegularExpressions;

namespace Floatingman.Common
{
    public static class StringExtensions
    {
        public static string ToKabobCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]}-{m.Value[1]}");
        }
    }
}