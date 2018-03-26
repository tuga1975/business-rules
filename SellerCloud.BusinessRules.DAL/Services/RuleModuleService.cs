using SellerCloud.BusinessRules.DAL.Repositories;
using SellerCloud.BusinessRules.Rules;
using SellerCloud.BusinessRules.Rules.RuleModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public class RuleModuleService : BusinessRulesEngineService<Rules.RuleModule.RuleModule, RuleModule, IRuleModuleRepository>, IRuleModuleService
    {
        private readonly IRuleService _ruleService;

        public RuleModuleService(IBusinessRulesEngineUnitOfWork unitOfWork, IRuleService ruleService)
            : base(unitOfWork, unitOfWork.RuleModuleRepository)
        {
            this._ruleService = ruleService;
        }

        protected override Rules.RuleModule.RuleModule PrepareDomainEntity(Rules.RuleModule.RuleModule ruleModule)
        {
            if (ruleModule != null)
            {
                if (ruleModule.Rules != null)
                {
                    foreach (Rules.Rule rule in ruleModule.Rules.Where(r => r is Rules.Rule))
                    {
                        if (rule.SaveArguments != null)
                        {
                            rule.Arguments = RuleArgumentConvertor.ConvertArguments(rule.SaveArguments);
                            rule.SaveArguments = null;
                        }
                    }
                }
            }

            return ruleModule;
        }

        public Task<List<Rules.RuleModule.RuleModule>> GetAllRoot(int clientId, int? idCompany = null)
        {
            var repository = this.Repository;
            return idCompany.HasValue
                ? repository.ListRootByCompanyAsync(idCompany.Value, clientId)
                : repository.ListRootAsync(clientId);
        }

        public Task<List<Rules.RuleModule.RuleModule>> ListByContextType(string contextType, int clientId) =>
            this.Repository.ListByContextType(contextType, clientId);

        public virtual async Task<List<Rules.RuleModule.RuleModule>> GetAllRootWithLinkedModules(int clientId)
        {
            Expression<Func<RuleModule, bool>> enabledPredicate = rm => rm.EnabledFlag == true;
            var ruleModules = this.Repository.ListRoot(clientId, enabledPredicate, true);
            var idRuleModules = ruleModules.Select(rm => rm.Id)
                .Concat(ruleModules.SelectMany(rm => rm.RuleModuleRootLink.Select(r => r.Id))).ToArray();

            var rules = await this._ruleService.ListByRuleModules(idRuleModules);

            return ruleModules
                .Select(rm => PopulateLinkedModules(rm, rm.RuleModuleRootLink, rules))
                .ToList();
        }

        private Rules.RuleModule.RuleModule FindLinkedModule(int idParent, RuleModulePath ruleModulePath, IEnumerable<Rules.RuleModule.RuleModule> children, IEnumerable<Rules.Rule> rules)
        {
            var ruleModule = children.FirstOrDefault(m => m.IdParentRuleModule == idParent && m.PathType == ruleModulePath);
            if (ruleModule != null)
            {
                return PopulateLinkedModules(ruleModule, children, rules);
            }

            return null;
        }

        private Rules.RuleModule.RuleModule PopulateLinkedModules(Rules.RuleModule.RuleModule parent, IEnumerable<Rules.RuleModule.RuleModule> children, IEnumerable<Rules.Rule> rules)
        {
            parent.Rules = rules.Where(r => r.IdRuleModule == parent.Id).ToArray();
            parent.IfTrue = FindLinkedModule(parent.Id, RuleModulePath.IfTrue, children, rules);
            parent.IfFalse = FindLinkedModule(parent.Id, RuleModulePath.IfFalse, children, rules);

            if (parent.RuleModuleType == RuleType.Action)
            {
                parent.Rules = parent.Rules.Concat(children
                    .Where(rm => rm.IdParentRuleModule == parent.Id)
                    .Select(rm => PopulateLinkedModules(rm, children, rules))).ToArray();
            }

            return parent;
        }

        public async Task<Rules.RuleModule.RuleModule> InquireWithLinkedModules(int id, bool loadRules = false)
        {
            var ruleModule = this.Repository.InquireRootWithChildren(id);
            if (ruleModule == null)
            {
                return ruleModule;
            }

            IEnumerable<Rules.Rule> rules = new List<Rules.Rule>();
            
            if (loadRules)
            {
                var idRuleModules = new int[] { ruleModule.Id }.Concat(ruleModule.RuleModuleRootLink.Select(rm => rm.Id)).ToArray();
                rules = await this._ruleService.ListByRuleModules(idRuleModules);
            }

            return PopulateLinkedModules(ruleModule, ruleModule.RuleModuleRootLink, rules);
        }

        public override Task<int> DeleteAsync(int id) => this.Repository.Delete(id);
    }

}
