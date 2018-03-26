using System.Collections.Generic;
using System.Threading.Tasks;
using SellerCloud.BusinessRules.DAL.Repositories;

namespace SellerCloud.BusinessRules.DAL.Services
{
    public class RuleArgumentService : BusinessRulesEngineService<Rules.RuleArgument, RuleArgument, IRuleArgumentRepository>, IRuleArgumentService
    {
        public RuleArgumentService(IBusinessRulesEngineUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.RuleArgumentRepository)
        {
        }

        public List<Rules.RuleArgument> ListByRule(int idRule) => this.Repository.ListByRule(idRule);
        public Task<List<Rules.RuleArgument>> ListByRuleAsync(int idRule) => this.Repository.ListByRuleAsync(idRule);
        
        public IEnumerable<int> DeleteAndCreate(int idRule, IEnumerable<Rules.RuleArgument> ruleArguments)
        {
            this.Delete(ruleArgument => ruleArgument.IdRule == idRule);
            return this.CreateRange(ruleArguments);
        }
    }
}
