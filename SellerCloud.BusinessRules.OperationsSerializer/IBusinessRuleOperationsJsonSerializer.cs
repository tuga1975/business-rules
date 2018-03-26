using System;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public interface IBusinessRuleOperationsJsonSerializer
    {
        string Serialize<TCustomMethods>() where TCustomMethods : class;
        string Serialize(Type customExtensionMethodsType = null);
    }
}