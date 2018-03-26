using SellerCloud.BusinessRules.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public interface IRuleArgumentService : IBusinessRulesEngineService<Rules.RuleArgument, RuleArgument, IRuleArgumentRepository>
    {
        Task<List<Rules.RuleArgument>> ListByRuleAsync(int idRule);
        IEnumerable<int> DeleteAndCreate(int idRule, IEnumerable<Rules.RuleArgument> ruleArguments);
    }

}
