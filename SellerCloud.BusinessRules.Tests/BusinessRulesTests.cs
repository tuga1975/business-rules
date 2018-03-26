using System.Collections.Generic;
using System.Linq;
using SellerCloud.BusinessRules.Models;
using SellerCloud.BusinessRules.Compilers;
using SellerCloud.BusinessRules.Logging;
using SellerCloud.BusinessRules.EntityTracking;
using SellerCloud.BusinessRules.Rules;
using NUnit.Framework;

namespace SellerCloud.BusinessRules.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public abstract class BusinessRulesTests
    {
        protected Order CreateOrder(double amount = 2.34334, string label = "", OrderShippingStatus shippingStatus = OrderShippingStatus.Unshipped) => 
            new Order
            {
                Amount = amount,
                Label = label
            };

        protected Order CreateOrder(IEnumerable<OrderItem> items) =>
            new Order
            {
                Amount = 0,
                Label = string.Empty,
                Items = items
            };

        protected OrderItem CreateOrderItem(string name = "") =>
            new OrderItem
            {
                Name = name
            };

        protected IEnumerable<OrderItem> CreateOrderItemsCollection(int count, string namePrefix = "") =>
            Enumerable.Range(0, count).Select(idx => new OrderItem
                {
                    Name = $"{ namePrefix }_{ idx }"
                });

        protected Address CreateAddress(string country = "", string state = "", string city = "", string zip = "", string street = "") =>
            new Address
            {
                Country = country,
                State = state,
                City = city,
                Zip = zip,
                Street = street
            };

        // INFO: By default log file is created in following path: D:\business.rules.engine.tests.log
        // You can edit App.config -> "file" attribute if you want to use different file name or path. 
        // Alternatively, DebugExpressionLogger can be used, which will print compiled expression in the debug view output
        private ILogger Logger => new FileExpressionLogger();

        private IEntityChangeTracker EntityChangeTracker => new EntityChangeTracker();

        protected IBooleanRuleCompiler BooleanRuleCompiler => new BooleanRuleCompiler();
        protected IBooleanRuleCompiler BooleanRuleCompilerWithLogger => new BooleanRuleCompiler(null, Logger);

        protected IActionRuleCompiler ActionRuleCompiler => new ActionRuleCompiler(EntityChangeTracker);
        protected IActionRuleCompiler ActionRuleCompilerWithLogger => new ActionRuleCompiler(EntityChangeTracker, null, Logger);

        protected IRuleProcessor RuleProcessor => new RuleProcessor(BooleanRuleCompiler, ActionRuleCompiler);

        private TRule CreateRule<TRule>(string expression, params RuleArgument[] arguments) where TRule : RuleBase, new() =>
            new TRule
            {
                Expression = expression,
                SaveArguments = arguments
            }.ConvertSaveArguments<TRule>();

        protected BooleanRule CreateBooleanRule(string expression, params RuleArgument[] arguments) => CreateRule<BooleanRule>(expression, arguments);

        protected ActionRule CreateActionRule(string expression, params RuleArgument[] arguments) => CreateRule<ActionRule>(expression, arguments);        
    }
}
