using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Validator
{
    public interface IConflictingRules
    {
        IEnumerable<IRuleModuleActionContainer> ActionRules { get; set; }
    }
}