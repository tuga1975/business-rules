using Newtonsoft.Json;
using SellerCloud.BusinessRules.TypeSerializer.TypeContainers;
using System;
using System.Linq;

namespace SellerCloud.BusinessRules.TypeSerializer.Converters
{
    public abstract class BusinessRulesJsonConverter : JsonConverter
    {
        protected readonly Type CustomExtensionMethodsType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected virtual void WriteDisplayNameProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            if (string.IsNullOrWhiteSpace(propertyInfoContainer.DisplayName)) return;

            writer.WritePropertyName("DisplayName");
            writer.WriteValue(propertyInfoContainer.DisplayName);
        }

        protected virtual void WritePropertyNameProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            if (string.IsNullOrWhiteSpace(propertyInfoContainer.Name)) return;

            writer.WritePropertyName("PropertyName");
            writer.WriteValue(propertyInfoContainer.Name);
        }

        protected virtual void WritePriorityProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            writer.WritePropertyName("Priority");
            writer.WriteValue(propertyInfoContainer.Priority);
        }

        protected virtual void WriteDescriptionProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            if (string.IsNullOrWhiteSpace(propertyInfoContainer.Description)) return;

            writer.WritePropertyName("Description");
            writer.WriteValue(propertyInfoContainer.Description);
        }

        private string GetTypeName(Type type)
        {
            if ((type.IsClass && type != typeof(string) && type != typeof(object)) || type.IsEnum)
            {
                return type.AssemblyQualifiedName;
            }

            return type.FullName;
        }

        protected virtual void WriteTypeProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            writer.WritePropertyName("Type");            
            writer.WriteValue(GetTypeName(propertyInfoContainer.Type));
        }

        protected virtual void WriteEndpointProperty(JsonWriter writer, PropertyInfoContainer propertyInfoContainer)
        {
            if (string.IsNullOrWhiteSpace(propertyInfoContainer.ItemsEndpoint)) return;

            writer.WritePropertyName("ItemsEndpoint");
            writer.WriteValue(propertyInfoContainer.ItemsEndpoint);
        }

        protected virtual void WriteAdditionalPropertyInformation(JsonWriter writer, PropertyInfoContainer propertyInfoContainer, JsonSerializer serializer)
        { }

        protected void WritePropertyInfo(JsonWriter writer, PropertyInfoContainer propertyInfoContainer, JsonSerializer serializer)
        {
            if (!propertyInfoContainer.Map) return;

            if (!string.IsNullOrEmpty(propertyInfoContainer.Name))
                writer.WritePropertyName(propertyInfoContainer.Name);

            writer.WriteStartObject();

            WriteDisplayNameProperty(writer, propertyInfoContainer);

            WritePropertyNameProperty(writer, propertyInfoContainer);

            WriteDescriptionProperty(writer, propertyInfoContainer);

            WritePriorityProperty(writer, propertyInfoContainer);

            WriteTypeProperty(writer, propertyInfoContainer);

            WriteEndpointProperty(writer, propertyInfoContainer);

            WriteAdditionalPropertyInformation(writer, propertyInfoContainer, serializer);

            writer.WriteEndObject();
        }
    }
}
