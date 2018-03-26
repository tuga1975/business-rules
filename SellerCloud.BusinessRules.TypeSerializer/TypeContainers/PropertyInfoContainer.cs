using SellerCloud.BusinessRules.Attributes;
using SellerCloud.BusinessRules.Serializer.Utils;
using System;
using System.Reflection;

namespace SellerCloud.BusinessRules.TypeSerializer.TypeContainers
{
    public abstract class PropertyInfoContainer : TypeInfoContainer
    {
        public string Name { get; private set; }
        public bool Map { get; private set; } = true;
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public string ItemsEndpoint { get; private set; }
        public int Priority { get; private set; }
        public bool IsValue { get; private set; }

        protected PropertyInfoContainer(Type type) : base(type)
        { }

        protected PropertyInfoContainer(PropertyInfo property) : base(property.PropertyType)
        {
            var brPropertyAttribute = property.GetCustomAttribute<BusinessRulePropertyAttribute>();
            var brItemsEndpointAttribute = property.GetCustomAttribute<BusinessRuleItemsEndpointAttribute>();

            Name = property.Name;
            Map = brPropertyAttribute != null;
            DisplayName = brPropertyAttribute?.Name ?? property.Name.CaseSplit();
            Description = brPropertyAttribute?.Description;
            ItemsEndpoint = brItemsEndpointAttribute?.Endpoint;
            Priority = brPropertyAttribute?.Priority ?? 0;
            IsValue = brPropertyAttribute?.IsValue ?? false;
        }

        public new static TypeInfoContainer Create(Type type)
        {
            var targetType = GetTargetContainerType(type, typeof(PropertyInfoContainer));
            return targetType != null ? (TypeInfoContainer)Activator.CreateInstance(targetType, type) : null;
        }

        public static PropertyInfoContainer Create(PropertyInfo property)
        {
            var targetType = GetTargetContainerType(property.PropertyType, typeof(PropertyInfoContainer));
            return targetType != null ? (PropertyInfoContainer)Activator.CreateInstance(targetType, property) : null;
        }
    }
}
