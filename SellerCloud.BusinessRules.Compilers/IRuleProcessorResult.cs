using SellerCloud.BusinessRules.EntityTracking;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Compilers
{
    public interface IRuleProcessorResult<T>
    {
        T Entity { get; }
        IEnumerable<int> EvaluationPath { get; }
        IEnumerable<IEntityChangeInformation> EntitiesChangeInformation { get; }
    }
}