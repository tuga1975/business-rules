using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Rules
{
    public interface IRule : ICloneable, IRuleCompilable
    {
        int IdRuleModule { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        ICollection<RuleArgument> Arguments { get; set; }
        ICollection<RuleArgument> SaveArguments { set; }
        string Expression { get; set; }
        RuleType RuleType { get; set; }
    }
}