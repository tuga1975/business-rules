using SellerCloud.BusinessRules.DAL.Mapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public class RuleRepository : GenericRepository<Rules.Rule, Rule>, IRuleRepository
    {
        public RuleRepository(IBusinessRulesEngineContext context) : base(context)
        { }

        public Task<List<Rules.Rule>> ListByRuleModule(int idRuleModule) =>
            FilterQuery(r => r.IdRuleModule == idRuleModule)
                .Include(r => r.Arguments)
                .OrderBy(r => r.Id)
                .ProjectToListAsync<Rule, Rules.Rule>()
                .InnerCollectionOrderBy(
                    r => r.Arguments,
                    arg => arg.Position
                );

        public Task<List<Rules.Rule>> ListByRuleModules(params int[] idRuleModules) =>
            FilterQuery(r => idRuleModules.Contains(r.IdRuleModule))
                .Include(r => r.Arguments)
                .OrderBy(r => r.IdRuleModule)
                .ThenBy(r => r.Id)
                .ProjectToListAsync<Rule, Rules.Rule>()
                .InnerCollectionOrderBy(
                    r => r.Arguments,
                    arg => arg.Position
                );

        public Task<Rules.Rule> InquireWithArguments(int id) =>
            InquireQuery(id)
                .Include(r => r.Arguments)
                .ProjectFirstOrDefaultAsync<Rule, Rules.Rule>()
                .InnerCollectionOrderBy(
                    r => r.Arguments,
                    arg => arg.Position
                );

        public override void Update(Rules.Rule rule)
        {
            var arguments = rule.Arguments;
            rule.Arguments = new List<Rules.RuleArgument>();

            base.Update(rule);

            rule.Arguments = arguments;
        }
    }
}
