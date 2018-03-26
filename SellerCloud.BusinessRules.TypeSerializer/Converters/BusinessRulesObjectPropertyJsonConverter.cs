using Newtonsoft.Json;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;

namespace SellerCloud.BusinessRules.TypeSerializer.Converters
{
    public class BusinessRulesObjectPropertyJsonConverter : BusinessRulesPropertyJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectPropertyInfoContainer);
        }

        protected override void WriteAdditionalPropertyInformation(JsonWriter writer, PropertyInfoContainer propertyInfoContainer, JsonSerializer serializer)
        {
            if (!propertyInfoContainer.IsValue)
            {
                var typeContainer = TypeInfoContainer.Create(propertyInfoContainer.Type);

                writer.WritePropertyName("TypeDefinition");

                serializer.Serialize(writer, typeContainer);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propertyInfoContainer = value as ObjectPropertyInfoContainer;

            this.WritePropertyInfo(writer, propertyInfoContainer, serializer);
        }
    }
}
