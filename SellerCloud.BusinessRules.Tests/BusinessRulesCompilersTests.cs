using System;
using NUnit.Framework;
using SellerCloud.BusinessRules.Models;
using SellerCloud.BusinessRules.Rules;

namespace SellerCloud.BusinessRules.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class BusinessRulesCompilersTests : BusinessRulesTests
    {
        [Test]
        public void BooleanCompiler_Should_Compile_To_Boolean_Lambda()
        {
            var rule = CreateBooleanRule(
                expression: "Label.Contains(@0)",
                arguments: new [] { new RuleArgument("Label") }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Contains_On_Inner_Property_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "ShippingAddress.Country.Contains(@0)",
                arguments: new [] { new RuleArgument("USA") }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Contains_On_Inner_Property_Of_Type_Collection_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Contains(Address.OtherInfo.Contains(@0))",
                arguments: new[] { new RuleArgument("Test") }
                );

            rule.Expression = "Items.Contains(Address.OtherInfo.Contains(@0))";

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Count_On_Inner_Property_Of_Type_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Count().Equals(@0)",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Sum_On_Inner_Property_Of_Type_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Sum(Address.Number).Equals(@0)",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Average_On_Inner_Property_Of_Type_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Average(Address.Number).Equals(@0)",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Min_On_Inner_Property_Of_Type_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Min(Address.Number).Equals(@0)",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Max_On_Inner_Property_Of_Type_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Max(Address.Number).Equals(@0)",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Count_On_Inner_Property_Of_Type_Collection_Within_Other_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Contains(Address.OtherInfo.Count().Equals(@0))",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Count_On_Inner_Object_Property_Of_Type_Collection_Within_Other_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Contains(Address.Tenantry.Count().Equals(@0))",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Sum_On_Inner_Object_Property_Of_Type_Collection_Within_Other_Collection_With_Collection_Specific_Function_Assigned()
        {
            var rule = CreateBooleanRule(
                expression: "Items.Contains(Address.Tenantry.Sum(Age).Equals(@0))",
                arguments: new[] { new RuleArgument(3) }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Equal_On_Inner_Property_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "ShippingAddress.Country.Equals(@0)",
                arguments: new [] { new RuleArgument("USA") }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_Between_Operator_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "Amount.Between(@0, @1)",
                arguments: new []
                    {
                        new RuleArgument(3),
                        new RuleArgument(6),
                    }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_BetweenOrEqual_Operator_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "Amount.BetweenOrEqual(@0, @1)",
                arguments: new[]
                    {
                        new RuleArgument(3),
                        new RuleArgument(6),
                    }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_In_Operator_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "ShippingStatus.IsInList(@0)",
                arguments: new []
                    {
                        new RuleArgument(new [] { OrderShippingStatus.FullyShipped, OrderShippingStatus.ReadyForPickup })
                        {
                            ValueType = typeof(OrderShippingStatus).AssemblyQualifiedName
                        }
                    }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void BooleanCompiler_Should_Compile_When_NotIn_Operator_Is_Used()
        {
            var rule = CreateBooleanRule(
                expression: "ShippingStatus.IsNotInList(@0)",
                arguments: new []
                    {
                        new RuleArgument(new [] { OrderShippingStatus.FullyShipped, OrderShippingStatus.ReadyForPickup })
                        {
                            ValueType = typeof(OrderShippingStatus).AssemblyQualifiedName
                        }
                    }
                );

            var compiled = BooleanRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiled);
            Assert.AreEqual(typeof(Func<Order, bool>), compiled.GetType());
        }

        [Test]
        public void ActionCompiler_Should_Compile_To_Generic_Lambda()
        {
            var rule = CreateActionRule(
                expression: "Amount.Round(@0)",
                arguments: new [] { new RuleArgument(2) }
                );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiledActionRule.Compiled);
            Assert.AreEqual(typeof(Action<Order>), compiledActionRule.Compiled.GetType());
        }

        [Test]
        public void ActionCompiler_Should_Compile_When_Property_From_Inner_Enumerable_Is_Used()
        {
            var rule = CreateActionRule(
                expression: "Items.Each(Address.OtherInfo.Each(Assign(@0)))",
                arguments: new[] { new RuleArgument("Test") }
                );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);

            Assert.IsNotNull(compiledActionRule);
            Assert.AreEqual(typeof(Action<Order>), compiledActionRule.Compiled.GetType());
        }

        [Test]
        public void ActionCompiler_Should_Compile_When_Filtered_Action_Is_Applied_On_Collection_Property()
        {
            var rule = CreateActionRule(
                expression: "Items.Filter(Address.Country.Equals(@0), Address.Country.Assign(@1))",
                arguments: new[] 
                    {
                        new RuleArgument("Canada"),
                        new RuleArgument("USA")
                    }
                );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule, true);

            Assert.IsNotNull(compiledActionRule);
            Assert.AreEqual(typeof(Action<Order>), compiledActionRule.Compiled.GetType());
        }
    }
}
