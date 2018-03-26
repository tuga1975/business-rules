using NUnit.Framework;
using SellerCloud.BusinessRules.Models;
using SellerCloud.BusinessRules.Rules;
using System.Linq;

namespace SellerCloud.BusinessRules.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class BooleanBusinessRulesTests : BusinessRulesTests
    {
        [Test]
        public void BooleanBusinessRule_Equal_Operator_Evaluation_Test()
        {
            var order = CreateOrder(label: "Test Label");

            var trueRule = CreateBooleanRule(
                expression: "Label.Equals(@0)",
                arguments: new [] { new RuleArgument(order.Label) }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument(order.Label + "_") };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_NotEqual_Operator_Evaluation_Test()
        {
            var order = CreateOrder(label: "Test Label");

            var trueRule = CreateBooleanRule(
                expression: "Label.NotEquals(@0)",
                arguments: new [] { new RuleArgument("Label") }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument(order.Label) };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_Contains_Operator_Evaluation_Test()
        {
            var order = CreateOrder(label: "Test Label");

            var trueRule = CreateBooleanRule(
                expression: "Label.Contains(@0)",
                arguments: new [] { new RuleArgument("Label") }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument("lbl") };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_StartsWith_Operator_Evaluation_Test()
        {
            var order = CreateOrder(label: "Test Label");

            var trueRule = CreateBooleanRule(
                expression: "Label.StartsWith(@0)",
                arguments: new [] { new RuleArgument("Test") }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument("Label") };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_EndsWith_Operator_Evaluation_Test()
        {
            var order = CreateOrder(label: "Test Label");

            var trueRule = CreateBooleanRule(
                expression: "Label.EndsWith(@0)",
                arguments: new [] { new RuleArgument("Label"), }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument("Test") };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_GreaterThan_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 5);

            var trueRule = CreateBooleanRule(
                expression: "Amount.GreaterThan(@0)",
                arguments: new [] { new RuleArgument(3) }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument(5) };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_GreaterThanOrEqual_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 5);

            var trueRule = CreateBooleanRule(
                expression: "Amount.GreaterThanOrEqual(@0)",
                arguments: new [] { new RuleArgument(5) }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument(6) };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_LessThan_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 5);

            var trueRule = CreateBooleanRule(
                expression: "Amount.LessThan(@0)",
                arguments: new[] { new RuleArgument(10) }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument(4) };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_LessThanOrEqual_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 5);

            var trueRule = CreateBooleanRule(
                expression: "Amount.LessThanOrEqual(@0)",
                arguments: new[] { new RuleArgument(5) }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument(4) };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_Between_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 4);

            var trueRule = CreateBooleanRule(
                expression: "Amount.Between(@0, @1)",
                arguments: new []
                    {
                        new RuleArgument(1),
                        new RuleArgument(10),
                    }
                );

            var falseRule = (BooleanRule)trueRule.Clone();
            falseRule.SaveArguments = new[] 
            {
                new RuleArgument(5),
                new RuleArgument(10)
            };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_BetweenOrEqual_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 1);

            var trueRule = CreateBooleanRule(
                expression: "Amount.BetweenOrEqual(@0, @1)",
                arguments: new[]
                    {
                        new RuleArgument(1),
                        new RuleArgument(10),
                    }
                );

            var falseRule = (BooleanRule)trueRule.Clone();
            falseRule.SaveArguments = new[]
            {
                new RuleArgument(5),
                new RuleArgument(10),
            };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_InList_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 5);

            var trueRule = CreateBooleanRule(
                expression: "Amount.IsInList(@0)",
                arguments: new[] 
                {
                    new RuleArgument(new[] { 1, 5, 10 }) { ValueType = typeof(string).FullName }
                }
            );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] 
            {
                new RuleArgument(new[] { 1, 4, 10 }) { ValueType = typeof(string).FullName }
            };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_NotInList_Operator_Evaluation_Test()
        {
            var order = CreateOrder(amount: 5);

            var trueRule = CreateBooleanRule(
                expression: "Amount.IsNotInList(@0)",
                arguments: new[] 
                {
                    new RuleArgument(new [] { 1, 4, 10 }) { ValueType = typeof(string).FullName }
                }
            );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] 
            {
                new RuleArgument(new[] { 1, 5, 10 }) { ValueType = typeof(string).FullName }
            };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        private Order CreateTestOrder() =>
            CreateOrder(CreateOrderItemsCollection(2, "Item")
                .Select(oi =>
                {
                    oi.Address = CreateAddress(country: "USA", state: "FL");
                    oi.Address.OtherInfo = Enumerable.Range(0, 2).Select(i => $"OtherInfo_{i}");
                    return oi;
                }));

        [Test]
        public void BooleanBusinessRule_Equal_Operator_On_Enumerable_Property_Evaluation_Test()
        {
            var order = CreateTestOrder();

            var trueRule = CreateBooleanRule(
                expression: "Items.Contains(Address.Country.Equals(@0))",
                arguments: new[] { new RuleArgument("USA") }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument("__") };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        [Test]
        public void BooleanBusinessRule_Contains_Operator_On_Deep_Enumerable_Property_Evaluation_Test()
        {
            var order = CreateTestOrder();

            var trueRule = CreateBooleanRule(
                expression: "Items.Contains(Address.OtherInfo.Contains(Contains(@0)))",
                arguments: new[] { new RuleArgument("_1") }
                );

            var falseRule = (IRule)trueRule.Clone();
            falseRule.SaveArguments = new[] { new RuleArgument("__") };

            TestBooleanRuleEvaluation(trueRule, falseRule, order);
        }

        protected void TestBooleanRuleEvaluation<TEntity>(IRule trueRule, IRule falseRule, TEntity entity)
            where TEntity : class, Models.IEntity, new()
        {
            ((RuleBase)trueRule).ConvertSaveArguments<RuleBase>();
            ((RuleBase)falseRule).ConvertSaveArguments<RuleBase>();

            var compiled = BooleanRuleCompilerWithLogger.Compile<TEntity>(trueRule);
            var result = compiled(entity);
            Assert.IsTrue(result);

            compiled = BooleanRuleCompilerWithLogger.Compile<TEntity>(falseRule);
            result = compiled(entity);
            Assert.IsFalse(result);
        }
    }
}
