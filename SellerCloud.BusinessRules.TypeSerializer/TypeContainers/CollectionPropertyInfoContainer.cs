using SellerCloud.BusinessRules.Extensions.Helpers;
using System;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public class CollectionPropertyInfoContainer : PropertyInfoContainer
    {
        public CollectionPropertyInfoContainer(Type type) : base(type)
        { }

        public CollectionPropertyInfoContainer(PropertyInfo property) : base(property)
        { }

        public static bool IsAllowedType(Type type) => type.IsCollection();
    }
}
