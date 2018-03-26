using SellerCloud.BusinessRules.DAL.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public class RuleArgumentRepository : GenericRepository<Rules.RuleArgument, RuleArgument>, IRuleArgumentRepository
    {
        public RuleArgumentRepository(IBusinessRulesEngineContext context) : base(context)
        { }

        public List<Rules.RuleArgument> ListByRule(int idRule) =>
            FilterQuery(ra => ra.IdRule == idRule)
                .ProjectToList<RuleArgument, Rules.RuleArgument>();

        public Task<List<Rules.RuleArgument>> ListByRuleAsync(int idRule) =>
            FilterQuery(ra => ra.IdRule == idRule)
                .ProjectToListAsync<RuleArgument, Rules.RuleArgument>();
    }

}
