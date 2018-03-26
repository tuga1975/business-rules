using System;
using SellerCloud.BusinessRules.Rules;

namespace SellerCloud.BusinessRules.Validator
{
    public interface IRuleModuleActionContainer
    {
        int IdRuleModule { get; set; }
        IRule Rule { get; set; }
    }
}