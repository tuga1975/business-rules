using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public interface IRuleArgumentRepository : IGenericRepository<Rules.RuleArgument, RuleArgument>
    {
        List<Rules.RuleArgument> ListByRule(int idRule);
        Task<List<Rules.RuleArgument>> ListByRuleAsync(int idRule);
    }

}
