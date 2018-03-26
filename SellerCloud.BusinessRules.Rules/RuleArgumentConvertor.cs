using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SellerCloud.BusinessRules.Extensions.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SellerCloud.BusinessRules.Rules
{
    public static class RuleArgumentConvertor
    {
        private static object ConvertValueToType(object value, Type valueType)
        {
            object contertedValue = null;

            try
            {
                if (valueType.IsEnum)
                {
                    contertedValue = Enum.Parse(valueType, value.ToString());
                }
            }
            catch (Exception)
            {
                contertedValue = Convert.ChangeType(value, valueType);
            }
            finally
            {
                contertedValue = contertedValue ?? Convert.ChangeType(value, valueType);
            }

            return contertedValue;
        }

        private static Type CreateCollectionFromType(Type valueType) => typeof(List<>).MakeGenericType(valueType);

        private static object ConvertValue(object value, Type valueType)
        {
            if (value == null)
            {
                return value;
            }

            if (value.GetType() == typeof(JObject))
            {
                var jObject = (JObject)value;                
                return jObject.ToObject(valueType);
            }

            if (value.GetType() == typeof(JArray))
            {
                var jArray = (JArray)value;
                return jArray.ToObject(CreateCollectionFromType(valueType));
            }

            if (value.GetType().IsCollection())
            {
                var collection = (IList)Activator.CreateInstance(CreateCollectionFromType(valueType));
                foreach (var item in (IList)value)
                {
                    var convertedValue = ConvertValue(item, valueType);
                    collection.Add(convertedValue);
                }
                return collection;
            }

            return ConvertValueToType(value, valueType);
        }

        private static string SerializeValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            string xml = string.Empty;
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream();

                var xw = XmlWriter.Create(ms);
                {
                    var type = value.GetType();
                    var serializer = new XmlSerializer(type);
                    serializer.Serialize(xw, value);
                    xw.Flush();
                }

                ms.Seek(0, SeekOrigin.Begin);

                using (var sr = new StreamReader(ms, System.Text.Encoding.UTF8))
                {
                    xml = sr.ReadToEnd();
                }
            }
            finally
            {
                ms?.Dispose();
            }

            return xml;
        }

        private static void ConvertArgument(RuleArgument argument)
        {
            var valueType = Type.GetType(argument.ValueType);
            var convertedValue = ConvertValue(argument.SaveValue, valueType);
            argument.ValueXml = SerializeValue(convertedValue);
            argument.ValueType = convertedValue?.GetType().AssemblyQualifiedName;
            argument.SaveValue = null;
        }

        public static ICollection<RuleArgument> ConvertArguments(ICollection<RuleArgument> arguments)
        {
            foreach (var argument in arguments)
            {
                ConvertArgument(argument);
            }

            return arguments;
        }
    }
}
