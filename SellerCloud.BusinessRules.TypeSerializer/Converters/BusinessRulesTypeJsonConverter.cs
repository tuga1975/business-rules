using Newtonsoft.Json;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.Converters
{
    public class BusinessRulesTypeJsonConverter : BusinessRulesJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TypeContainer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var typeContainer = value as TypeContainer;
            var type = typeContainer.Type;

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            writer.WriteStartObject();

            foreach (var property in properties)
            {
                var propertyInfoContainer = PropertyInfoContainer.Create(property);
                if (propertyInfoContainer != null)
                {
                    serializer.Serialize(writer, propertyInfoContainer);
                }
            }

            writer.WriteEndObject();
        }
    }
}
