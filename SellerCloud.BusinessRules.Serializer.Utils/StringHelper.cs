using System.Text.RegularExpressions;

namespace SellerCloud.BusinessRules.Serializer.Utils
{
    public static class StringHelper
    {
        public static string CaseSplit(this string source)
        {
            if (source == null) return source;

            var upperCaseLetterProcessedString = Regex.Replace(source, "([a-z0-9])([A-Z])", "$1 $2");
            return Regex.Replace(upperCaseLetterProcessedString, "([a-z])([0-9])", "$1 $2");
        }
    }
}
