using System;

namespace SellerCloud.BusinessRules.Rules
{
    public interface IRuleArgument : ICloneable
    {
        int IdRule { get; set; }
        object Value { get; }
        object SaveValue { set; }
        string ValueXml { get; set; }
        string ValueType { get; set; }
        int Position { get; set; }

        Type GetValueType();
    }
}