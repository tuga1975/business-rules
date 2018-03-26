using SellerCloud.BusinessRules.Extensions.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.OperationsSerializer.Converters
{
    public class CollectionOfTypesJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IEnumerable<Type>).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private string GetTypeName(Type type)
        {
            if ((type.IsClass && type != typeof(string) && type != typeof(object)) || type.IsEnum)
            {
                return type.AssemblyQualifiedName;
            }

            return type.FullName;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var types = value as IEnumerable<Type>;

            writer.WriteStartArray();

            foreach (var type in types)
            {
                if (type.IsCollection())
                {
                    writer.WriteValue("Collection<>");
                }
                else if (type.IsFunction())
                {
                    var funcArgumentTypes = type.GenericTypeArguments.Select(arg => 
                    {
                        var implementedTypes = arg.GetImplementedSystemTypes(TypesContainer.ImplementedSystemTypes);
                        return $"[{ string.Join("; ", implementedTypes.Select(t => t.FullName)) }]";
                    });
                    
                    writer.WriteValue($"Function<{ string.Join(", ", funcArgumentTypes) }>");
                }
                else
                {
                    writer.WriteValue(this.GetTypeName(type));
                }
            }

            writer.WriteEndArray();
        }
    }
}
