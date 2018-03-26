using System.Reflection;
using System.Runtime.CompilerServices;

namespace SellerCloud.BusinessRules.Extensions.Helpers
{
    public static class MethodInfoHelper
    {
        public static bool IsExtensionMethod(this MethodInfo source)
        {
            return source.GetCustomAttribute<ExtensionAttribute>() != null;
        }
    }
}
