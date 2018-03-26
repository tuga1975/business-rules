using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public abstract class TypeInfoContainer
    {
        public Type Type { get; private set; }

        protected TypeInfoContainer(Type type)
        {
            Type = type;
        }

        private static IEnumerable<Type> PropertyInfoContainerTypes
        {
            get
            {
                var baseType = typeof(TypeInfoContainer);
                var types = baseType.Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && t.IsSubclassOf(baseType));
                return types;
            }
        }

        protected static Type GetTargetContainerType(Type type, Type baseType) =>
            PropertyInfoContainerTypes
                .Where(t => t.BaseType == baseType)
                .FirstOrDefault(t =>
                {
                    var isAllowedTypeMethod = t.GetMethod("IsAllowedType", BindingFlags.Static | BindingFlags.Public);
                    if (isAllowedTypeMethod == null) return false;

                    return (bool)isAllowedTypeMethod.Invoke(null, new object[] { type });
                });

        public static TypeInfoContainer Create(Type type)
        {
            var targetType = GetTargetContainerType(type, typeof(TypeInfoContainer));
            return targetType != null ? (TypeInfoContainer)Activator.CreateInstance(targetType, type) : null;
        }
    }
}
