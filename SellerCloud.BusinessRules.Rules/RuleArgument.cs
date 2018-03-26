using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SellerCloud.BusinessRules.Rules
{
    public class RuleArgument : IEntity, IRuleArgument
    {
        public int Id { get; set; }
        
        public object Value
        {
            get
            {
                try
                {
                    return DeserializeValue(ValueXml, Type.GetType(ValueType));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public object SaveValue { get; set; }

        [JsonIgnore]
        public string ValueXml { get; set; }

        public string ValueType { get; set; }

        public string ValueKey { get; set; }

        public int Position { get; set; }

        public int IdRule { get; set; }

        public string Description { get; set; } = "";

        private object DeserializeValue(string xml, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(xml))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public RuleArgument()
        {
        }

        public RuleArgument(object value) : this()
        {
            SaveValue = value;
            ValueType = value?.GetType().FullName;
        } 

        public object Clone() => this.MemberwiseClone();

        public Type GetValueType() => Value.GetType();

        public override string ToString()
        {
            if (Value is string value)
            {
                return $"'{ value }'";
            }

            return Value.ToString();
        }
    }
}
