using Newtonsoft.Json;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;

namespace SellerCloud.BusinessRules.TypeSerializer.Converters
{
    public class BusinessRulesEnumPropertyJsonConverter : BusinessRulesPropertyJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EnumPropertyInfoContainer);
        }

        protected override void WriteAdditionalPropertyInformation(JsonWriter writer, PropertyInfoContainer propertyInfoContainer, JsonSerializer serializer)
        {
            writer.WritePropertyName("EnumQualifiedName");

            writer.WriteValue(propertyInfoContainer.Type.AssemblyQualifiedName);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propertyInfoContainer = value as EnumPropertyInfoContainer;

            this.WritePropertyInfo(writer, propertyInfoContainer, serializer);
        }
    }
}
