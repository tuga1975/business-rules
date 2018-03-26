using SellerCloud.BusinessRules.Rules;

namespace SellerCloud.BusinessRules.DAL
{
    public partial class RuleModule : IRuleCompilable
    {
        public RuleCompilableType Type => RuleCompilableType.Module;
    }

    public partial class Rule : IRuleCompilable
    {
        public RuleCompilableType Type => RuleCompilableType.Rule;
    }

    public partial class RuleArgument : IEntity
    {
    }

    public partial class RuleModuleCompanyLink : IEntity
    {
    }
}
