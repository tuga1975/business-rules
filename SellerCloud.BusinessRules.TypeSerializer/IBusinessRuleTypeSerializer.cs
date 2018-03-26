using System;

namespace SellerCloud.BusinessRules.TypeSerializer
{
    public interface IBusinessRuleTypeSerializer
    {
        string Serialize<T>() where T : class;
        string Serialize(Type type);
    }
}