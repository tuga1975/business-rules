using Newtonsoft.Json;
using System;

namespace SellerCloud.BusinessRules.OperationsSerializer.Converters
{
    public class BusinessRulesOperationsContainer : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(OperationsContainer);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            writer.WriteStartObject();

            var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var property in properties)
            {
                writer.WritePropertyName(property.Name);
                var propertyValue = property.GetValue(value);
                serializer.Serialize(writer, propertyValue);
            }

            writer.WriteEndObject();
        }
    }
}
