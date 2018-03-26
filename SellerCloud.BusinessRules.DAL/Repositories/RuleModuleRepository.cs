using SellerCloud.BusinessRules.DAL.Mapper;
using SellerCloud.BusinessRules.Rules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Repositories
{
    public class RuleModuleRepository : GenericRepository<Rules.RuleModule.RuleModule, RuleModule>, IRuleModuleRepository
    {
        public RuleModuleRepository(IBusinessRulesEngineContext context) : base(context)
        {
            BusinessRuleMapper.Instance.Init(
                new BusinessRuleMapperTarget(typeof(IRuleCompilable), typeof(RuleModule), typeof(Rules.RuleModule.RuleModule)),
                new BusinessRuleMapperTarget(typeof(IRuleCompilable), typeof(Rule), typeof(Rules.Rule))
            );
        }

        private IQueryable<RuleModule> ListRootQuery(int clientId, int? idCompany = null, bool includeChildren = false)
        {
            var query = FilterQuery(rm => !rm.IdRootRuleModule.HasValue && rm.ClientID == clientId);

            if (idCompany.HasValue)
            { 
                query = query.Where(rm =>
                    rm.RuleModuleCompanyLinks.Any(rmcl => rmcl.IdRuleModule == rm.Id && rmcl.IdCompany == idCompany) ||
                    !rm.RuleModuleCompanyLinks.Any(rmcl => rmcl.IdRuleModule == rm.Id)
                );
            }

            if (includeChildren)
            {
                query = query.Include(rm => rm.RuleModuleRootLink);
            }

            return query.OrderBy(rm => rm.Name);
        }

        public IEnumerable<Rules.RuleModule.RuleModule> ListRoot(int clientId, Expression<Func<RuleModule, bool>> predicate, bool includeChildren = false)
        {
            var query = ListRootQuery(clientId, null, includeChildren);
            return query
                .Where(predicate)
                .OrderBy(rm => rm.Id)
                .ProjectToList<RuleModule, Rules.RuleModule.RuleModule>();
        }

        public IEnumerable<Rules.RuleModule.RuleModule> ListRoot(int clientId, bool includeChildren = false)
        {
            var query = ListRootQuery(clientId, null, includeChildren);
            return query.ProjectToList<RuleModule, Rules.RuleModule.RuleModule>();
        }
        
        public Task<List<Rules.RuleModule.RuleModule>> ListRootAsync(int clientId, bool includeChildren = false)
        {
            var query = ListRootQuery(clientId, null, includeChildren);
            return query.ProjectToListAsync<RuleModule, Rules.RuleModule.RuleModule>();
        }

        public Task<List<Rules.RuleModule.RuleModule>> ListByContextType(string contextType, int clientId) =>
            FilterQuery(rm => !rm.IdRootRuleModule.HasValue && rm.ContextType == contextType && rm.ClientID == clientId)
                .OrderBy(rm => rm.Name)
                .ProjectToListAsync<RuleModule, Rules.RuleModule.RuleModule>();

        public List<Rules.RuleModule.RuleModule> ListRootByCompany(int idCompany, int clientId, bool includeChildren = false)
        {
            var query = ListRootQuery(clientId, idCompany, includeChildren);
            return query.ProjectToList<RuleModule, Rules.RuleModule.RuleModule>();
        }

        public Task<List<Rules.RuleModule.RuleModule>> ListRootByCompanyAsync(int idCompany, int clientId, bool includeChildren = false)
        {
            var query = ListRootQuery(clientId, idCompany, includeChildren);
            return query.ProjectToListAsync<RuleModule, Rules.RuleModule.RuleModule>();
        }

        public IQueryable<RuleModule> ListByRootRuleModuleQuery(int id) =>
            FilterQuery(rm => rm.IdRootRuleModule == id);

        public List<Rules.RuleModule.RuleModule> ListByRootRuleModule(int id) =>
            ListByRootRuleModuleQuery(id)
                .ProjectToList<RuleModule, Rules.RuleModule.RuleModule>();

        public Task<List<Rules.RuleModule.RuleModule>> ListByRootRuleModuleAsync(int id) =>
            ListByRootRuleModuleQuery(id)
                .ProjectToListAsync<RuleModule, Rules.RuleModule.RuleModule>();


        protected override IQueryable<RuleModule> InquireQuery(int id) =>
            FilterQuery(rm => rm.Id == id)
                .Include(nameof(RuleModule.Rules))
                .Include($"{nameof(RuleModule.Rules)}.{nameof(Rule.Arguments)}");

        public Rules.RuleModule.RuleModule InquireRootWithChildren(int idRootRuleModule) =>
            FilterQuery(rm => !rm.IdRootRuleModule.HasValue && rm.Id == idRootRuleModule)
                .Include(rm => rm.RuleModuleRootLink)
                .ProjectFirstOrDefault<RuleModule, Rules.RuleModule.RuleModule>();

        public override RuleModule Add(Rules.RuleModule.RuleModule domain)
        {
            var dtoEntity = BusinessRuleMapper.Instance.Map<Rules.RuleModule.RuleModule, RuleModule>(domain);
            if (domain.RootRuleModule == domain.ParentRuleModule)
            {
                dtoEntity.RootRuleModule = dtoEntity.ParentRuleModule;
            }

            Set().Add(dtoEntity);

            return dtoEntity;
        }

        public Task<int> Delete(int id)
        {
            return this.Context.Database.ExecuteSqlCommandAsync("[BusinessRulesEngine].[RuleModuleDelete] @Id", new SqlParameter("@Id", id));
        }
    }

}
