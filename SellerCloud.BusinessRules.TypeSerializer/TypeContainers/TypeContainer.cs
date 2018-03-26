using System;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public class TypeContainer : TypeInfoContainer
    {
        public TypeContainer(Type type) : base(type)
        { }

        public static bool IsAllowedType(Type type) => true;
    }
}
