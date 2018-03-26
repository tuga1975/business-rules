using SellerCloud.BusinessRules.Extensions.Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using SellerCloud.BusinessRules.Serializer.Utils;

namespace SellerCloud.BusinessRules.OperationsSerializer.Converters
{
    public class BusinessRuleOperationJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(BusinessRuleOperation).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name != nameof(BusinessRuleOperation.Ignore));

            writer.WriteStartObject();

            foreach (var property in properties)
            {
                writer.WritePropertyName(property.Name);
                var propertyValue = property.GetValue(value);

                if (property.Name == nameof(BusinessRuleOperation.DisplayName))
                {
                    var displayName = (propertyValue as string)?.CaseSplit();
                    writer.WriteValue(displayName);
                }
                else if (property.Name == nameof(BusinessRuleOperation.ReturnType))
                {
                    var returnType = (Type)property.GetValue(value);
                    if (returnType.IsCollection())
                    {
                        var collectionType = returnType.GetCollectionElementType();
                        writer.WriteValue($"Collection<{ collectionType.FullName ?? "" }>");
                    }
                    else
                    {
                        writer.WriteValue(returnType.FullName ?? "");
                    }
                }
                else
                {
                    serializer.Serialize(writer, propertyValue, property.PropertyType);
                }
            }

            writer.WriteEndObject();
        }
    }
}
