using SellerCloud.BusinessRules.OperationsSerializer;
using SellerCloud.BusinessRules.TypeSerializer;
using System;

namespace SellerCloud.BusinessRules.Serializer.Facade
{
    public class BusinessRulesSerializerFacade
    {
        private readonly IBusinessRuleTypeSerializer businessRuleTypeSerializer;
        private readonly IBusinessRuleOperationsJsonSerializer businessRuleOperationsJsonSerializer;

        public BusinessRulesSerializerFacade(IBusinessRuleTypeSerializer businessRuleTypeSerializer, IBusinessRuleOperationsJsonSerializer businessRuleOperationsJsonSerializer)
        {
            this.businessRuleTypeSerializer = businessRuleTypeSerializer;
            this.businessRuleOperationsJsonSerializer = businessRuleOperationsJsonSerializer;
        }

        public string SerializeEntityTypeToJson<T>() where T : class => 
            businessRuleTypeSerializer.Serialize<T>();

        public string SerializeEntityTypeToJson(Type type) => 
            businessRuleTypeSerializer.Serialize(type);

        public string SerializeOperationsToJson<TCustomMethods>() where TCustomMethods : class => 
            businessRuleOperationsJsonSerializer.Serialize<TCustomMethods>();

        public string SerializeOperationsToJson(Type customMethodsType = null) => 
            businessRuleOperationsJsonSerializer.Serialize(customMethodsType);
    }
}
