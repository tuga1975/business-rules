using SellerCloud.BusinessRules.Extensions.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.DAL.Mapper
{
    public class BusinessRuleMapper
    {
        private BusinessRuleMapperConfiguration _configuration;

        private static readonly Lazy<BusinessRuleMapper> lazy = new Lazy<BusinessRuleMapper>(() => new BusinessRuleMapper());

        public static BusinessRuleMapper Instance => lazy.Value;

        private BusinessRuleMapper()
        {
            this._configuration = new BusinessRuleMapperConfiguration();
        }

        public void Init(BusinessRuleMapperConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void Init(params BusinessRuleMapperTarget[] configurationTargets)
        {
            if (configurationTargets != null)
            {
                this._configuration.Targets = new HashSet<BusinessRuleMapperTarget>(this._configuration.Targets.Concat(configurationTargets));
            }
        }

        private object CreateCollectionValue(object value, Type sourceType, Type destType, Dictionary<object, object> convertedObjects = null)
        {
            if (value == null)
            {
                return value;
            }

            IList collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(destType));

            foreach (var item in value as IEnumerable)
            {
                object collectionItem = item;
                var itemType = sourceType.IsInterface ? item.GetType() : sourceType;

                if (itemType.GetTypeInfo().IsClass && itemType != typeof(string))
                {
                    if (convertedObjects.ContainsKey(item))
                    {
                        collectionItem = convertedObjects[item];
                    }
                    else
                    {
                        collectionItem = ConvertObject(item, null, itemType, destType, convertedObjects);
                    }
                }

                collection.Add(collectionItem);
            }

            return collection;
        }

        private object ConvertObject(object source, object dest, Type sourceType, Type destType, Dictionary<object, object> convertedObjects = null)
        {
            var mapMethodInfo = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == "Map")
                .First(m => !m.GetParameters().First().ParameterType.IsCollection());

            var configurationTarget = this._configuration.Targets.FirstOrDefault(t => t.Target == destType && t.Source == sourceType);
            destType = configurationTarget?.Dest ?? destType;

            return Map(sourceType, destType, source, dest, convertedObjects);
        }

        public object Map(Type sourceType, Type destType, object source, object dest = null, Dictionary<object, object> convertedObjects = null)
        {
            if (source == null)
            {
                return dest;
            }

            convertedObjects = convertedObjects ?? new Dictionary<object, object>();

            if (convertedObjects.ContainsKey(source))
            {
                return convertedObjects[source];
            }

            if (dest == null)
            {
                dest = BLToolkit.Mapping.Map.ObjectToObject(source, destType);
            }
            else
            {
                dest = BLToolkit.Mapping.Map.ObjectToObject(source, dest);
            }
            var destProperties = destType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanWrite)
                .Where(p => p.PropertyType.IsCollection() || (p.PropertyType.GetTypeInfo().IsClass && p.PropertyType != typeof(string)));

            convertedObjects.Add(source, dest);

            foreach (var destProperty in destProperties)
            {
                var destValue = destProperty.CanRead ? destProperty.GetValue(dest) : null;

                var sourceProperty = sourceType.GetProperty(destProperty.Name, BindingFlags.Instance | BindingFlags.Public);
                if (sourceProperty != null)
                {
                    var value = sourceProperty.GetValue(source);

                    if (value != null)
                    {
                        if (sourceProperty.PropertyType.IsCollection())
                        {
                            value = CreateCollectionValue(value, sourceProperty.PropertyType.GetCollectionElementType(), destProperty.PropertyType.GetCollectionElementType(), convertedObjects);
                        }
                        else
                        {
                            if (convertedObjects.ContainsKey(value))
                            {
                                value = convertedObjects[value];
                            }
                            else
                            {
                                value = ConvertObject(value, destValue, sourceProperty.PropertyType, destProperty.PropertyType, convertedObjects);
                            }
                        }
                    }

                    destProperty.SetValue(dest, value);
                }
                else if (destProperty.PropertyType == typeof(object) && destValue?.GetType() == typeof(BLToolkit.Reflection.InitContext))
                {
                    destProperty.SetValue(dest, null);
                }
            }

            return dest;
        }

        public TDest Map<TSource, TDest>(TSource source, TDest dest = null, Dictionary<object, object> convertedObjects = null)
            where TDest : class
            where TSource : class
        {
            var sourceType = typeof(TSource);
            var destType = typeof(TDest);
            var mappedObject = Map(sourceType, destType, source, dest, convertedObjects);

            return (TDest)mappedObject;
        }

        public IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> sourceCollection, Dictionary<object, object> convertedObjects = null)
            where TDest : class
            where TSource : class
        {
            return sourceCollection.Select(item => Map<TSource, TDest>(item, null, convertedObjects));
        }
    }

}
