using System.Collections.Generic;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public class OperationsContainer
    {
        public IEnumerable<BusinessRuleOperation> BooleanOperations { get; set; }
        public IEnumerable<BusinessRuleOperation> ActionOperations { get; set; }
        public IEnumerable<BusinessRuleOperation> CollectionOperations { get; set; }
    }
}
