using System.Linq;
using SellerCloud.BusinessRules.Rules.RuleModule;
using SellerCloud.BusinessRules.Rules;
using SellerCloud.BusinessRules.Models;
using SellerCloud.BusinessRules.Validator;
using NUnit.Framework;

namespace SellerCloud.BusinessRules.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class BusinessRuleValidatorTests : BusinessRulesTests
    {
        private RuleModule moduleChain;

        [SetUp]
        public void Init()
        {
            InitModuleChain();
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
            var roundAmountWith1Action = CreateActionRule(
                    expression: "Amount.Round(@0)",
                    arguments: new[] { new RuleArgument(1) }
                );
            var roundAmountWith2Action = CreateActionRule(
                    expression: "Amount.Round(@0)",
                    arguments: new[] { new RuleArgument(2) }
                );
            var add10ToAmountAction = CreateActionRule(
                    expression: "Amount.Add(@0)",
                    arguments: new[] { new RuleArgument(10) }
                );
            var add20ToAmountAction = CreateActionRule(
                    expression: "Amount.Add(@0)",
                    arguments: new[] { new RuleArgument(20) }
                );
            var changeShippingStatusToFullyShippedAction = CreateActionRule(
                    expression: "ShippingStatus.Assign(@0)",
                    arguments: new[] { new RuleArgument(OrderShippingStatus.FullyShipped) { ValueType = typeof(OrderShippingStatus).AssemblyQualifiedName } }
                );
            var updateOrderItemNameToSent = CreateActionRule(
                    expression: "Items.Each(Name.Assign(@0))",
                    arguments: new[] { new RuleArgument("Sent") }
                );
            var updateOrderItemNameToNotSent = CreateActionRule(
                    expression: "Items.Each(Name.Assign(@0))",
                    arguments: new[] { new RuleArgument("NotSent") }
                );

            moduleChain = new RuleModule(RuleType.Boolean, ordersWithLabelContainingOrder, ordersWithAmountGreaterThan500)
            {
                BooleanEvalLogic = BooleanRuleModuleEvalLogic.Or,
                IfFalse = new RuleModule(RuleType.Boolean, ordersFromCalifornia)
                {
                    BooleanEvalLogic = BooleanRuleModuleEvalLogic.And,
                    IfFalse = new RuleModule(RuleType.Action, roundAmountWith1Action, roundAmountWith2Action),
                    IfTrue = new RuleModule(RuleType.Action, add10ToAmountAction, add20ToAmountAction)
                },
                IfTrue = new RuleModule(RuleType.Action, updateOrderItemNameToSent, updateOrderItemNameToNotSent, changeShippingStatusToFullyShippedAction)
            };
        }

        [Test]
        public void RuleProcessor_With_Single_Module_Should_Not_Throw_Exception()
        {
            var validator = new RulesTreeValidator(ActionRuleCompiler);
            var result = validator.Validate<Order>(moduleChain);
            Assert.AreEqual(3, result.Count());

            Assert.IsTrue(result.All(r => r.Count() == 1));

            var conflictingActions = result.SelectMany(r => r.Select(arc => arc.ActionRules).SelectMany(ar => ar)).ToArray();
            Assert.AreEqual(6, conflictingActions.Length);
            Assert.AreEqual(2, conflictingActions.Where(a => a.Rule.Expression.StartsWith("Amount.Add")).Count());
            Assert.AreEqual(2, conflictingActions.Where(a => a.Rule.Expression.StartsWith("Amount.Round")).Count());
            Assert.AreEqual(2, conflictingActions.Where(a => a.Rule.Expression.StartsWith("Items.Each(Name.Assign")).Count());
        }
    }
}
