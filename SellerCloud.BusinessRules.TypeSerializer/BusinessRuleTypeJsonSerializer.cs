using Newtonsoft.Json;
using SellerCloud.BusinessRules.Serializer.Utils;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer
{
    public class BusinessRuleTypeJsonSerializer : IBusinessRuleTypeSerializer
    {
        public string Serialize<T>() where T : class
        {
            var type = typeof(T);
            return Serialize(type);
        }

        public string Serialize(Type type)
        {
            var typeContainer = TypeInfoContainer.Create(type);

            var converters = JsonHelper.GetAssemblyDefinedConverters(Assembly.GetExecutingAssembly()).ToList();
            converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            var json = JsonConvert.SerializeObject(typeContainer, Formatting.Indented, converters.ToArray());
            return json;
        }
    }
}
