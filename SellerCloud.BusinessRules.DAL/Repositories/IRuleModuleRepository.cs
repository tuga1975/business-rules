using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public interface IRuleModuleRepository : IGenericRepository<Rules.RuleModule.RuleModule, RuleModule>
    {
        IEnumerable<Rules.RuleModule.RuleModule> ListRoot(int clientId, Expression<Func<RuleModule, bool>> predicate, bool includeChildren = false);
        IEnumerable<Rules.RuleModule.RuleModule> ListRoot(int clientId, bool includeChildren = false);
        Task<List<Rules.RuleModule.RuleModule>> ListRootAsync(int clientId, bool includeChildren = false);
        Task<List<Rules.RuleModule.RuleModule>> ListByContextType(string contextType, int clientId);
        List<Rules.RuleModule.RuleModule> ListRootByCompany(int idCompany, int clientId, bool includeChildren = false);
        Task<List<Rules.RuleModule.RuleModule>> ListRootByCompanyAsync(int idCompany, int clientId, bool includeChildren = false);
        List<Rules.RuleModule.RuleModule> ListByRootRuleModule(int id);
        Task<List<Rules.RuleModule.RuleModule>> ListByRootRuleModuleAsync(int id);
        Rules.RuleModule.RuleModule InquireRootWithChildren(int idRootRuleModule);
        Task<int> Delete(int id);
    }

}
