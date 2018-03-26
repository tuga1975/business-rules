using System;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public class EnumPropertyInfoContainer : PropertyInfoContainer
    {
        public EnumPropertyInfoContainer(Type type) : base(type)
        { }

        public EnumPropertyInfoContainer(PropertyInfo property) : base(property)
        { }

        public static bool IsAllowedType(Type type)
        {
            return type.IsEnum;
        }
    }
}
