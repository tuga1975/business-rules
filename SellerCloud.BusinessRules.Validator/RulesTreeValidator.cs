using SellerCloud.BusinessRules.Compilers;
using SellerCloud.BusinessRules.EntityTracking;
using SellerCloud.BusinessRules.Rules.RuleModule;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.Validator
{
    public class RulesTreeValidator
    {
        private readonly IActionRuleCompiler actionRuleCompiler;

        public RulesTreeValidator(IActionRuleCompiler actionRuleCompiler)
        {
            this.actionRuleCompiler = actionRuleCompiler;
        }

        public IEnumerable<IEnumerable<IConflictingRules>> Validate<T>(RuleModule ruleModule)
        {
            var actionRulesSet = new HashSet<IRuleModuleActionContainer>();
            var actionGroups = new HashSet<IEnumerable<IRuleModuleActionContainer>> { actionRulesSet };
            var ruleModuleValidator = new RuleModuleValidator(actionGroups);
            ruleModuleValidator.FindActionRules(ruleModule, actionRulesSet);

            var res = actionGroups.AsParallel().Select(GetForConflicts<T>);
            return res.Where(l => l.Any()).ToList();
        }

        private IEnumerable<IConflictingRules> GetForConflicts<T>(IEnumerable<IRuleModuleActionContainer> actionRuleContainers)
        {
            var conflictingRulesDictionary = new Dictionary<IEntityChangeInformation, HashSet<IRuleModuleActionContainer>>();

            foreach (var actionRuleContainer in actionRuleContainers)
            {
                var compiledActionRule = actionRuleCompiler.Compile<T>(actionRuleContainer.Rule, true);
                if (conflictingRulesDictionary.ContainsKey(compiledActionRule.EntityChangeInformation))
                {
                    conflictingRulesDictionary[compiledActionRule.EntityChangeInformation].Add(actionRuleContainer);
                }
                else
                {
                    conflictingRulesDictionary.Add(compiledActionRule.EntityChangeInformation, new HashSet<IRuleModuleActionContainer> { actionRuleContainer });
                }
            }

            var conflictingRules = conflictingRulesDictionary.Where(kvp => kvp.Value.Count > 1).Select(kvp => new ConflictingRules(kvp.Value));
            return conflictingRules;
        }
    }
}
