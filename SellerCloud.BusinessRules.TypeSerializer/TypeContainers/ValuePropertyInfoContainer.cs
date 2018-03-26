using System;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public class ValuePropertyInfoContainer : PropertyInfoContainer
    {
        public ValuePropertyInfoContainer(Type type) : base(type)
        { }

        public ValuePropertyInfoContainer(PropertyInfo property) : base(property)
        { }

        public static bool IsAllowedType(Type type)
        {
            if (type.IsClass)
            {
                return type == typeof(string) || type == typeof(DateTime);
            }

            return type.IsValueType;
        }
    }
}
