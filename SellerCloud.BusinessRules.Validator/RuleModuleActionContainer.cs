using SellerCloud.BusinessRules.Rules;
using System.Linq;

namespace SellerCloud.BusinessRules.Validator
{
    public class RuleModuleActionContainer : IRuleModuleActionContainer
    {
        public int IdRuleModule { get; set; }
        public IRule Rule { get; set; }

        public RuleModuleActionContainer(int idRuleModule, IRule rule)
        {
            IdRuleModule = idRuleModule;
            Rule = rule;
        }

        public override string ToString()
        {
            var ruleArguments = Rule.Arguments.ToArray();
            var ruleExpression = Rule.Expression;

            for (int i = 0; i < ruleArguments.Length; i++)
            {
                var arg = ruleArguments[i];
                ruleExpression = ruleExpression.Replace($"@{i}", arg.ToString());
            }

            return $"{ IdRuleModule } -> { Rule.Name } - { ruleExpression }";
        }
    }
}
