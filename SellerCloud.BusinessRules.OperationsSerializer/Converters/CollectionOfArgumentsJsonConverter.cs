using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.OperationsSerializer.Converters
{
    public class CollectionOfArgumentsJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IEnumerable<BusinessRuleOperationArgument>).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var arguments = value as IEnumerable<BusinessRuleOperationArgument>;

            writer.WriteStartArray();

            var argsGrouped = arguments
                .SelectMany(a => a.ApplicableTypes.Select(t => new { a.Position, Type = t }))
                .GroupBy(a => a.Position);

            foreach (var group in argsGrouped)
            {
                var position = group.Key;
                var applicableTypes = group.Select(arg => arg.Type).Distinct();

                writer.WriteStartObject();

                writer.WritePropertyName(nameof(BusinessRuleOperationArgument.Position));
                writer.WriteValue(position);

                writer.WritePropertyName(nameof(BusinessRuleOperationArgument.ApplicableTypes));
                serializer.Serialize(writer, applicableTypes);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
