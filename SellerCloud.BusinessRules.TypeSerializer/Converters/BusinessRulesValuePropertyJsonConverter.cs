using Newtonsoft.Json;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;

namespace SellerCloud.BusinessRules.TypeSerializer.Converters
{
    public class BusinessRulesValuePropertyJsonConverter : BusinessRulesPropertyJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ValuePropertyInfoContainer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propertyInfoContainer = value as ValuePropertyInfoContainer;

            this.WritePropertyInfo(writer, propertyInfoContainer, serializer);
        }
    }
}
