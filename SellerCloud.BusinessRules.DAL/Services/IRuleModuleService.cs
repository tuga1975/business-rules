using SellerCloud.BusinessRules.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public interface IRuleModuleService : IBusinessRulesEngineService<Rules.RuleModule.RuleModule, RuleModule, IRuleModuleRepository>
    {
        Task<List<Rules.RuleModule.RuleModule>> GetAllRoot(int clientId, int? idCompany = null);
        Task<List<Rules.RuleModule.RuleModule>> ListByContextType(string contextType, int clientId);
        Task<List<Rules.RuleModule.RuleModule>> GetAllRootWithLinkedModules(int clientId);
        Task<Rules.RuleModule.RuleModule> InquireWithLinkedModules(int id, bool loadRules = false);
    }

}
