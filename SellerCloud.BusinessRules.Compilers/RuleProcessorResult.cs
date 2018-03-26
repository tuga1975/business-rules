using SellerCloud.BusinessRules.EntityTracking;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Compilers
{
    public class RuleProcessorResult<T> : IRuleProcessorResult<T>
    {
        public T Entity { get; private set; }
        public IEnumerable<int> EvaluationPath { get; private set; } = new HashSet<int>();
        public IEnumerable<IEntityChangeInformation> EntitiesChangeInformation { get; private set; } = new List<IEntityChangeInformation>();

        public RuleProcessorResult(T entity)
        {
            this.Entity = entity;
        }

        public RuleProcessorResult(T entity, IEnumerable<IEntityChangeInformation> entitiesChangeInformation)
            : this(entity)
        {
            this.EntitiesChangeInformation = entitiesChangeInformation;
        }

        public RuleProcessorResult(T entity, IEnumerable<int> evaluationPath, IEnumerable<IEntityChangeInformation> entitiesChangeInformation) 
            : this(entity)
        {
            if (evaluationPath != null)
            {
                this.EvaluationPath = new HashSet<int>(evaluationPath);
            }

            this.EntitiesChangeInformation = entitiesChangeInformation;
        }
    }
}