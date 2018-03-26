using Newtonsoft.Json;
using SellerCloud.BusinessRules.Extensions.Helpers;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;

namespace SellerCloud.BusinessRules.TypeSerializer.Converters
{
    public class BusinessRulesCollectionPropertyJsonConverter : BusinessRulesPropertyJsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CollectionPropertyInfoContainer);
        }

        protected override void WriteTypeProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            writer.WritePropertyName("Type");
            writer.WriteValue("Collection");
        }

        protected override void WriteAdditionalPropertyInformation(JsonWriter writer, PropertyInfoContainer propertyInfoContainer, JsonSerializer serializer)
        {
            var type = propertyInfoContainer.Type.GetCollectionElementType();
            var typeContainer = PropertyInfoContainer.Create(type);

            writer.WritePropertyName("TypeDefinition");

            serializer.Serialize(writer, typeContainer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propertyInfoContainer = value as CollectionPropertyInfoContainer;

            this.WritePropertyInfo(writer, propertyInfoContainer, serializer);
        }
    }
}
