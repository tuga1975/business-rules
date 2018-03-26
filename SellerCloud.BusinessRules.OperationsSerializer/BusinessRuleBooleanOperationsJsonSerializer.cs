using Newtonsoft.Json;
using SellerCloud.BusinessRules.Attributes;
using SellerCloud.BusinessRules.Extensions.ExtensionMethods;
using SellerCloud.BusinessRules.Extensions.Helpers;
using SellerCloud.BusinessRules.Serializer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public class BusinessRuleOperationsJsonSerializer : IBusinessRuleOperationsJsonSerializer
    {
        private IEnumerable<MethodInfo> GetMethods(Type type) => type?.GetStaticMethods() ?? Enumerable.Empty<MethodInfo>();

        private IEnumerable<Type> GetApplicableTypesNames(BusinessRuleApplicabilityScopeAttribute attr, ParameterInfo parameter) => 
            (attr?.ApplicableTypes.Any() ?? false)
                ? attr?.ApplicableTypes 
                : parameter.ParameterType.GetImplementedSystemTypes(TypesContainer.ImplementedSystemTypes);

        private IEnumerable<BusinessRuleOperation> GetBusinessRuleMethodOperations(IEnumerable<MethodInfo> methods)
        {
            var operationsSet = new HashSet<BusinessRuleOperation>();

            foreach (var method in methods)
            {
                var brApplicabilityScopeAttr = method.GetCustomAttribute<BusinessRuleApplicabilityScopeAttribute>();
                var brActionMethodAttr = method.GetCustomAttribute<BusinessRuleActionMethodAttribute>();
                var brBooleanMethodAttr = method.GetCustomAttribute<BusinessRuleBooleanMethodAttribute>();
                var brBusinessRuleItemsEndpointAttr = method.GetCustomAttribute<BusinessRuleItemsEndpointAttribute>();

                if (brApplicabilityScopeAttr?.Ignore ?? false)
                {
                    continue;
                }

                var parameters = method.GetParameters();
                var extensionMethodObjectTypeParam = parameters.First(p => p.Position == 0);
                var argumentParameters = parameters.SkipWhile(p => p.Position == 0);

                var existingOperation = operationsSet.FirstOrDefault(o => o.Operation == method.Name && Enumerable.Count(o.Arguments) == Enumerable.Count(argumentParameters));

                if (existingOperation != null)
                {                    
                    existingOperation.ApplicableTypes = existingOperation.ApplicableTypes.Concat(GetApplicableTypesNames(brApplicabilityScopeAttr, extensionMethodObjectTypeParam)).Distinct();
                    existingOperation.Arguments = existingOperation.Arguments.MergeParameters(argumentParameters);
                }
                else
                {
                    var businessRuleBooleanOperation = new BusinessRuleOperation
                    {
                        Operation = method.Name,
                        DisplayName = brApplicabilityScopeAttr?.Name ?? method.Name,
                        Ignore = brApplicabilityScopeAttr?.Ignore ?? false,
                        ApplicableTypes = GetApplicableTypesNames(brApplicabilityScopeAttr, extensionMethodObjectTypeParam),
                        ReturnType = method.ReturnType,
                        ItemsEndpoint = brBusinessRuleItemsEndpointAttr?.Endpoint,
                        Arguments = argumentParameters
                            .Select((p, idx) => {
                                var argument = new BusinessRuleOperationArgument { Position = idx };

                                var applicableTypes = p.ParameterType.GetImplementedSystemTypes(TypesContainer.ImplementedSystemTypes);

                                if ((p.ParameterType.IsCollection() || p.ParameterType.IsFunction()))
                                {
                                    applicableTypes = Enumerable.Repeat(p.ParameterType, 1);
                                }

                                argument.ApplicableTypes = argument.ApplicableTypes.Concat(applicableTypes);

                                return argument;
                            }).ToList()
                    };

                    if (brActionMethodAttr != null || brBooleanMethodAttr != null)
                    {
                        businessRuleBooleanOperation.RuleType = brActionMethodAttr != null ? Rules.RuleType.Action : Rules.RuleType.Boolean;
                    }

                    operationsSet.Add(businessRuleBooleanOperation);
                }
            }

            return operationsSet;
        }

        private IEnumerable<BusinessRuleOperation> GetOperations(Type targetType, Type customExtensionMethodsType = null) 
        {
            var methods = GetMethods(targetType).Concat(GetMethods(customExtensionMethodsType));
            return GetBusinessRuleMethodOperations(methods);
        }

        public string Serialize<TCustomMethods>() where TCustomMethods : class
        {
            var customExtensionMethodsType = typeof(TCustomMethods);
            return Serialize(customExtensionMethodsType);
        }

        public string Serialize(Type customExtensionMethodsType = null)
        {
            var booleanOperations = GetOperations(typeof(BooleanMethods), customExtensionMethodsType);
            var actionOperations = GetOperations(typeof(ActionMethods), customExtensionMethodsType);
            var collectionOperations = GetOperations(typeof(CollectionMethods), customExtensionMethodsType);

            var container = new OperationsContainer
            {
                BooleanOperations = booleanOperations,
                ActionOperations = actionOperations,
                CollectionOperations = collectionOperations
            };

            var converters = JsonHelper.GetAssemblyDefinedConverters(Assembly.GetExecutingAssembly()).ToList();
            converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            var json = JsonConvert.SerializeObject(container, Formatting.Indented, converters.ToArray());
            return json;
        }
    }
}
