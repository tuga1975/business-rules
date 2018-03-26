using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Rules.RuleModule
{
    public interface IRuleModule : IRuleCompilable
    {
        string Name { get; set; }
        int? IdRootRuleModule { get; set; }
        int? IdParentRuleModule { get; set; }
        int? CompanyID { get; set; }
        RuleType RuleModuleType { get; }
        RuleModulePath? PathType { get; set; }
        ICollection<IRuleCompilable> Rules { get; set; }
        BooleanRuleModuleEvalLogic? BooleanEvalLogic { get; set; }
        string ContextType { get; set; }
    }
}