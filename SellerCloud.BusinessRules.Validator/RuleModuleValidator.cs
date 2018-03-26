using SellerCloud.BusinessRules.Rules;
using SellerCloud.BusinessRules.Rules.RuleModule;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Validator
{
    public class RuleModuleValidator
    {
        private readonly HashSet<IEnumerable<IRuleModuleActionContainer>> actionGroups;

        public RuleModuleValidator(HashSet<IEnumerable<IRuleModuleActionContainer>> actionGroups)
        {
            this.actionGroups = actionGroups;
        }

        public void FindActionRules(RuleModule ruleModule, HashSet<IRuleModuleActionContainer> actionRules)
        {
            if (ruleModule.RuleModuleType == RuleType.Boolean)
            {
                if (ruleModule.IfTrue != null)
                {
                    FindActionRules(ruleModule.IfTrue, actionRules);
                }

                if (ruleModule.IfFalse != null)
                {
                    if (ruleModule.IfTrue != null)
                    {
                        actionRules = new HashSet<IRuleModuleActionContainer>();
                        actionGroups.Add(actionRules);
                    }

                    FindActionRules(ruleModule.IfFalse, actionRules);
                }
            }
            else
            {
                FindActionRulesFromSet(ruleModule, actionRules);
            }
        }

        private void FindActionRulesFromSet(RuleModule ruleModule, HashSet<IRuleModuleActionContainer> actionRules)
        {
            foreach (var rule in ruleModule.Rules)
            {
                if (rule.Type == RuleCompilableType.Module)
                {
                    FindActionRules(rule as RuleModule, actionRules);
                }
                else
                {
                    var ruleModuleActionContainer = new RuleModuleActionContainer(ruleModule.Id, rule as IRule);
                    actionRules.Add(ruleModuleActionContainer);
                }
            }
        }
    }
}
