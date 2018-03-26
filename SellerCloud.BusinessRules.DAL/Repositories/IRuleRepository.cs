using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public interface IRuleRepository : IGenericRepository<Rules.Rule, Rule>
    {
        Task<List<Rules.Rule>> ListByRuleModule(int idRuleModule);
        Task<List<Rules.Rule>> ListByRuleModules(int[] idRuleModules);
        Task<Rules.Rule> InquireWithArguments(int id);
    }

}
