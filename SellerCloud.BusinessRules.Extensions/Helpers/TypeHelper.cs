using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.Extensions.Helpers
{
    public static class TypeHelper
    {
        public static bool ImplementsType<TTarget>(this Type source)
        {
            var targetType = typeof(TTarget);
            return ImplementsType(source, targetType);
        }

        public static bool ImplementsType(this Type source, Type targetType)
        {
            if (!targetType.IsInterface)
                return false;

            var targetTypeAsEnumerable = Enumerable.Repeat(targetType, 1);

            var types = targetTypeAsEnumerable
                .Concat(targetTypeAsEnumerable.SelectMany(t => t.GetInterfaces()))
                .Distinct();

            return source.GetInterfaces().Any(t => types.Any(i => i == t));
        }

        public static bool IsNullableType(this Type source) => source.IsClass || Nullable.GetUnderlyingType(source) != null;

        public static bool IsCollection(this Type source) => source != typeof(string) && (source.ImplementsType(typeof(IEnumerable)) || source == typeof(IEnumerable));

        public static bool IsFunction(this Type source) => source.IsSubclassOf(typeof(MulticastDelegate)) && source.GetMethod("Invoke").ReturnType != typeof(void);

        public static bool IsAction(this Type source) => source.IsSubclassOf(typeof(MulticastDelegate)) && source.GetMethod("Invoke").ReturnType == typeof(void);

        public static IEnumerable<MethodInfo> GetStaticMethods(this Type source) => source.GetMethods(BindingFlags.Public | BindingFlags.Static);

        public static Type GetGenericType(this Type source) => source.IsGenericType ? source.GenericTypeArguments.First() : source;

        public static Type GetCollectionElementType(this Type source) => source.IsArray ? source.GetElementType() : source.GenericTypeArguments.FirstOrDefault();

        public static IEnumerable<Type> GetImplementedSystemTypes(this Type source, IEnumerable<Type> targetSystemTypes)
        {
            var type = source.GetGenericType();
            var typeInfo = type.GetTypeInfo();

            if (!type.IsGenericParameter) return Enumerable.Repeat(type, 1);

            if (!typeInfo.ImplementedInterfaces.Any(i => !i.IsCollection()))
            {
                if (type.IsGenericParameter) return Enumerable.Repeat(typeof(object), 1);

                return Enumerable.Repeat(type, 1);
            }

            return typeof(string).Assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => typeInfo.ImplementedInterfaces.All(i => i.IsAssignableFrom(t)))
                .Where(t => targetSystemTypes.Any(st => st == t));
        }

        public static Type GetEnumType(this Type source)
        {
            if (source.IsEnum)
            {
                return source;
            }

            var sourceNullableType = Nullable.GetUnderlyingType(source);

            if (sourceNullableType?.IsEnum ?? false)
            {
                return sourceNullableType;
            }

            return null;
        }
    }
}
