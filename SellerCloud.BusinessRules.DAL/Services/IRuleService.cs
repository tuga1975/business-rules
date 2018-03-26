using SellerCloud.BusinessRules.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public interface IRuleService : IBusinessRulesEngineService<Rules.Rule, Rule, IRuleRepository>
    {
        Task<List<Rules.Rule>> ListByRuleModule(int idRuleModule);
        Task<List<Rules.Rule>> ListByRuleModules(params int[] idRuleModules);
        Task<Rules.Rule> InquireWithArguments(int id);
    }
}
