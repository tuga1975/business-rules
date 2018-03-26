using SellerCloud.BusinessRules.EntityTracking;
using SellerCloud.BusinessRules.Rules.RuleModule;
using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Compilers
{
    public interface IRuleProcessor
    {
        IRuleProcessorResult<T> ProcessMany<T>(IEnumerable<RuleModule> modules, T entity);
        IRuleProcessorResult<T> Process<T>(RuleModule module, T entity, HashSet<IEntityChangeInformation> entitiesChangeInformationSet = null, bool applyActions = true);
    }

    public interface IRuleProcessorFactory
    {
        IRuleProcessor CreateRuleProcessor();
    }

    public class RuleProcessorFactory : IRuleProcessorFactory
    {
        private readonly Func<IRuleProcessor> _factory;

        public RuleProcessorFactory(Func<IRuleProcessor> factory)
        {
            this._factory = factory;
        }

        public IRuleProcessor CreateRuleProcessor()
        {
            return this._factory();
        }
    }
}