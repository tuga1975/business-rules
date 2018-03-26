using System.Linq;
using Newtonsoft.Json;
using SellerCloud.BusinessRules.Compilers;
using SellerCloud.BusinessRules.Rules.RuleModule;
using SellerCloud.BusinessRules.Rules;
using SellerCloud.BusinessRules.Models;
using System.Collections.Generic;
using NUnit.Framework;

namespace SellerCloud.BusinessRules.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class BusinessRuleProcessorTests : BusinessRulesTests
    {
        private RuleModule singleModule;
        private RuleModule moduleChain;

        [SetUp]
        public void Init()
        {
            InitSingleModule();
            InitModuleChain();
        }

        private void InitSingleModule()
        {
            var booleanRules = new []
            {
                CreateBooleanRule(
                    expression: "Label.Contains(@0)", 
                    arguments: new [] { new RuleArgument("Order") }
                ),
                CreateBooleanRule(
                    expression: "Label.Contains(@0)", 
                    arguments: new [] { new RuleArgument("Test Order") }
                ),
            };
            
            var actionRules = new []
            {
                CreateActionRule(
                    expression: "Amount.Round(@0)",
                    arguments: new [] { new RuleArgument(2) }
                ),
                CreateActionRule(
                    expression: "Amount.Add(@0)",
                    arguments: new [] { new RuleArgument(10) }
                ),
            };

            singleModule = new RuleModule(RuleType.Boolean, booleanRules)
            {
                BooleanEvalLogic = BooleanRuleModuleEvalLogic.And,
                IfTrue = new RuleModule(RuleType.Action, actionRules)
            };
        }

        private void InitModuleChain()
        {
            // Boolean rules
            var ordersWithLabelContainingOrder = CreateBooleanRule(
                    expression: "Label.Contains(@0)",
                    arguments: new[] { new RuleArgument("Order") }
                );
            var ordersWithAmountGreaterThan500 = CreateBooleanRule(
                    expression: "Amount.GreaterThan(@0)",
                    arguments: new[] { new RuleArgument(500) }
                );
            var ordersFromCalifornia = CreateBooleanRule(
                    expression: "FromAddress.State.Equals(@0)",
                    arguments: new[] { new RuleArgument("CA") }
                );

            // Action rules
            var roundAmountAction = CreateActionRule(
                    expression: "Amount.Round(@0)",
                    arguments: new[] { new RuleArgument(2) }
                );
            var add10ToAmountAction = CreateActionRule(
                    expression: "Amount.Add(@0)",
                    arguments: new[] { new RuleArgument(10) }
                );
            var changeShippingStatusToFullyShippedAction = CreateActionRule(
                    expression: "ShippingStatus.Assign(@0)",
                    arguments: new[] { new RuleArgument(OrderShippingStatus.FullyShipped) { ValueType = typeof(OrderShippingStatus).AssemblyQualifiedName } }
                );
            var updateOrderItemNameToSent = CreateActionRule(
                    expression: "Items.Each(Name.Assign(@0))",
                    arguments: new[] { new RuleArgument("Sent") }
                );

            moduleChain = new RuleModule(RuleType.Boolean, ordersWithLabelContainingOrder, ordersWithAmountGreaterThan500)
            {
                BooleanEvalLogic = BooleanRuleModuleEvalLogic.Or,
                IfFalse = new RuleModule
                {
                    RuleModuleType = RuleType.Boolean,
                    BooleanEvalLogic = BooleanRuleModuleEvalLogic.And,
                    Rules = new HashSet<IRuleCompilable> { ordersFromCalifornia },
                    IfFalse = new RuleModule(RuleType.Action, roundAmountAction),
                    IfTrue = new RuleModule(RuleType.Action, add10ToAmountAction)
                },
                IfTrue = new RuleModule(RuleType.Action, updateOrderItemNameToSent, changeShippingStatusToFullyShippedAction)
            };
        }

        private Order CreateOrder()
        {
            var order = CreateOrder(amount: 340.54321, label: "Test Order", shippingStatus: OrderShippingStatus.Unshipped);
            order.Items = CreateOrderItemsCollection(5, "Item");
            order.ShippingAddress = CreateAddress(country: "USA", state: "CA", city: "San Francisco");
            return order;
        }

        [Test]
        public void RuleProcessor_With_Single_Module_Should_Not_Throw_Exception()
        {
            var order = CreateOrder();
            Assert.AreEqual(340.54321, order.Amount);

            var processor = RuleProcessor;
            var result = processor.Process(singleModule, order);

            Assert.AreEqual(350.54, result.Entity.Amount);
            Assert.IsTrue(result.EvaluationPath.Any());
        }

        [Test]
        public void RuleProcessor_With_ModuleChain_Should_Not_Throw_Exception()
        {
            var order = CreateOrder();
            var processor = RuleProcessor;
            var result = processor.Process(moduleChain, order);

            Assert.IsNotNull(result.Entity);
            Assert.IsTrue(result.EvaluationPath.Any());
        }

        private void TestEquality<T>(IRuleProcessorResult<T> expected, IRuleProcessorResult<T> actual)
        {
            var expectedEntityAsJson = JsonConvert.SerializeObject(expected.Entity);
            var actualEntityAsJson = JsonConvert.SerializeObject(actual.Entity);

            Assert.AreEqual(expectedEntityAsJson, actualEntityAsJson);
            CollectionAssert.AreEqual(expected.EvaluationPath.ToArray(), actual.EvaluationPath.ToArray());
        }
    }
}
