using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.OperationsSerializer.Converters
{
    public class BusinessRuleOperationsJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IEnumerable<BusinessRuleOperation>).IsAssignableFrom(objectType);

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

            writer.WriteStartArray();

            var operations = value as IEnumerable<BusinessRuleOperation>;
            foreach (var operation in operations)
            {
                if (operation.Ignore) continue;

                serializer.Serialize(writer, operation);
            }

            writer.WriteEndArray();
        }
    }
}
