using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.Serializer.Utils
{
    public static class JsonHelper
    {
        public static JsonConverter[] GetAssemblyDefinedConverters(Assembly assembly)
        {
            var baseType = typeof(JsonConverter);
            var types = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType))
                .Where(t => !t.IsAbstract)
                .Select(t => (JsonConverter)Activator.CreateInstance(t));
            return types.ToArray();
        }
    }
}
