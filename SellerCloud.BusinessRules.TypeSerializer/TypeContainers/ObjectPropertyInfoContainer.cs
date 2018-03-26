using System;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public class ObjectPropertyInfoContainer : PropertyInfoContainer
    {
        public ObjectPropertyInfoContainer(Type type) : base(type)
        { }

        public ObjectPropertyInfoContainer(PropertyInfo property) : base(property)
        { }

        public static bool IsAllowedType(Type type)
        {
            if (type.IsClass && !type.IsEnum)
            {
                return type != typeof(string) && type != typeof(DateTime);
            }

            return false;
        }
    }
}
