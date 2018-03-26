using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SellerCloud.BusinessRules.DAL.Repositories;
using SellerCloud.BusinessRules.Rules;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public class RuleService : BusinessRulesEngineService<Rules.Rule, Rule, IRuleRepository>, IRuleService
    {
        private readonly IRuleArgumentService _ruleArgumentService;

        public RuleService(IBusinessRulesEngineUnitOfWork unitOfWork, IRuleArgumentService ruleArgumentService) 
            : base(unitOfWork, unitOfWork.RuleRepository)
        {
            this._ruleArgumentService = ruleArgumentService;
        }

        protected override Rules.Rule PrepareDomainEntity(Rules.Rule rule)
        {
            if (rule != null)
            {
                if (rule.SaveArguments != null)
                {
                    rule.Arguments = RuleArgumentConvertor.ConvertArguments(rule.SaveArguments);
                    rule.SaveArguments = null;
                }
            }

            return rule;
        }

        public Task<List<Rules.Rule>> ListByRuleModule(int idRuleModule) => this.Repository.ListByRuleModule(idRuleModule);

        public Task<List<Rules.Rule>> ListByRuleModules(params int[] idRuleModules) => this.Repository.ListByRuleModules(idRuleModules);
        public Task<Rules.Rule> InquireWithArguments(int id) => this.Repository.InquireWithArguments(id);
        
        protected override void UpdateEntity(Rules.Rule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            var updateArguments = rule.SaveArguments != null;

            rule = PrepareDomainEntity(rule);

            if (updateArguments)
            {
                this._ruleArgumentService.DeleteAndCreate(rule.Id, rule.Arguments);
            }

            this.Repository.Update(rule);
        }
    }
}
