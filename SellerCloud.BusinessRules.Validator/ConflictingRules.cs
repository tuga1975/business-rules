using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Validator
{
    public class ConflictingRules : IConflictingRules
    {
        public IEnumerable<IRuleModuleActionContainer> ActionRules { get; set; }

        public ConflictingRules(IEnumerable<IRuleModuleActionContainer> actionRules)
        {
            ActionRules = actionRules;
        }
    }
}
