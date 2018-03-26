using System.Collections.Generic;
using System.Threading.Tasks;
using SellerCloud.BusinessRules.Compilers;
using SellerCloud.BusinessRules.Rules.RuleModule;

namespace SellerCloud.BusinessRules.Facade
{
    public interface IBusinessRulesFacade
    {
        RuleModule GetRuleModule(int idRootRuleModule);
        Task LoadRuleModules(int clientId);
        IRuleProcessorResult<T> Process<T>(RuleModule rootRuleModule, T entity);
        IRuleProcessorResult<T> ProcessAll<T>(T entity);
        IRuleProcessorResult<T> ProcessAll<T>(IEnumerable<RuleModule> rootRuleModules, T entity, IRuleProcessor ruleProcessor = null);
        IEnumerable<IRuleProcessorResult<T>> ProcessAllEntities<T>(IEnumerable<T> entities);
    }
}