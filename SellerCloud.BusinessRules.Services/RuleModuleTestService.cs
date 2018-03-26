using SellerCloud.BusinessRules.Compilers;
using SellerCloud.BusinessRules.DAL.Services;
using SellerCloud.ShipUI.EntityContext;
using ShipUI.Facade;
using System;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Persistence
{
    public class RuleModuleTestService
    {
        private readonly IRuleProcessor ruleProcessor;
        private readonly IRuleModuleService _ruleModuleService;
        private readonly IUnitOfWork unitOfWork;

        public RuleModuleTestService(IBooleanRuleCompiler booleanRuleCompiler, IActionRuleCompiler actionRuleCompiler, IUnitOfWork unitOfWork, IRuleModuleService ruleModuleService)
        {
            ruleProcessor = new RuleProcessor(booleanRuleCompiler, actionRuleCompiler);
            this.unitOfWork = unitOfWork;
            this._ruleModuleService = ruleModuleService;
        }

        public async Task<IRuleProcessorResult<EntityContext>> Process(int id, int orderId)
        {
            var ruleModule = await this._ruleModuleService.InquireWithLinkedModules(id, true);
            var order = this.unitOfWork.OrderService.Repository
                .CreateQueryAsNoTracking()
                .Where(o => o.ID == orderId)
                .FirstOrDefault();

            if (order == null)
            {
                throw new ArgumentException($"Order with ID: {orderId} not found");
            }

            var entityContext = new EntityContext
            {
                Order = order
            };

            var result = ruleProcessor.Process(ruleModule, entityContext, null, false);
            return result;
        }
    }
}
