using NUnit.Framework;
using SellerCloud.BusinessRules.Models;
using SellerCloud.BusinessRules.Rules;
using System.Linq;

namespace SellerCloud.BusinessRules.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class ActionBusinessRulesTests : BusinessRulesTests
    {
        [Test]
        public void ActionBusinessRule_Assign_Test()
        {
            var order = CreateOrder(amount: 2.54321);
            var rule = CreateActionRule(
                expression: "Amount.Assign(@0)",
                arguments: new[] { new RuleArgument(6) }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(6, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Round_Test()
        {
            var order = CreateOrder(amount: 2.54321);
            var rule = CreateActionRule(
                //memberName: nameof(Order.Amount),
                expression: "Amount.Round(@0)",
                arguments: new [] { new RuleArgument(2) }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(2.54, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Ceiling_Test()
        {
            var order = CreateOrder(amount: 2.54321);
            var rule = CreateActionRule(
                expression: "Amount.Ceiling()"
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(3, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Floor_Test()
        {
            var order = CreateOrder(amount: 2.54321);
            var rule = CreateActionRule(
                expression: "Amount.Floor()"
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(2, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Add_Test()
        {
            var order = CreateOrder(amount: 2);
            var rule = CreateActionRule(
                expression: "Amount.Add(@0)",
                arguments: new [] { new RuleArgument(2), new RuleArgument(32) }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(4, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Subtract_Test()
        {
            var order = CreateOrder(amount: 4);
            var rule = CreateActionRule(
                expression: "Amount.Subtract(@0)",
                arguments: new[] { new RuleArgument(2) }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(2, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Multiply_Test()
        {
            var order = CreateOrder(amount: 4);
            var rule = CreateActionRule(
                expression: "Amount.Multiply(@0)",
                arguments: new[] { new RuleArgument(2) }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(8, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Divide_Test()
        {
            var order = CreateOrder(amount: 6);
            var rule = CreateActionRule(
                expression: "Amount.Divide(@0)",
                arguments: new[] { new RuleArgument(2) }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(3, order.Amount);
        }

        [Test]
        public void ActionBusinessRule_Trim_Test()
        {
            var order = CreateOrder(amount: 6, label: " Test Label ");
            var rule = CreateActionRule(
                expression: "Label.Trim()"
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual(order.Label.Trim(), order.Label);
        }

        [Test]
        public void ActionBusinessRule_Concat_Test()
        {
            var order = CreateOrder(amount: 6, label: "Test Label");
            var rule = CreateActionRule(
                expression: "Label.Concat(@0)",
                arguments: new[] { new RuleArgument(" - concatenated string") }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);
            Assert.AreEqual("Test Label - concatenated string", order.Label);
        }

        private Order CreateTestOrder() =>
            CreateOrder(CreateOrderItemsCollection(2, "Item")
                .Select(oi =>
                {
                    oi.Address = CreateAddress(country: "USA", state: "FL");
                    return oi;
                }));

        [Test]
        public void ActionBusinessRule_Assign_On_Inner_Enumerable_Property()
        {
            var order = CreateTestOrder();

            var rule = CreateActionRule(
                expression: "Items.Each(Address.State.Assign(@0))",
                arguments: new[] { new RuleArgument("CA") }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);

            foreach (var item in order.Items)
            {
                Assert.AreEqual("CA", item.Address.State);
            }
        }

        [Test]
        public void ActionBusinessRule_Concat_On_Inner_Enumerable_Property()
        {
            var order = CreateTestOrder();

            var rule = CreateActionRule(
                expression: "Items.Each(Address.State.Concat(@0))",
                arguments: new[] { new RuleArgument("_CA") }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);

            foreach (var item in order.Items)
            {
                Assert.AreEqual("FL_CA", item.Address.State);
            }
        }

        [Test]
        public void ActionBusinessRule_Concat_On_Inner_Enumerable_Of_Scalar_Values()
        {
            var orderItems = CreateOrderItemsCollection(2, "Item")
                .Select(oi =>
                {
                    oi.Address = CreateAddress(country: "USA", state: "FL");
                    oi.Address.OtherInfo = Enumerable.Range(0, 2).Select(i => $"OtherInfo_{i}");
                    return oi;
                });
            var order = CreateOrder(orderItems);

            foreach (var item in order.Items)
            {
                Assert.AreEqual("FL", item.Address.State);
            }

            var rule = CreateActionRule(
                expression: "Items.Each(Address.OtherInfo.Each(Assign(@0)))",
                arguments: new[] { new RuleArgument("_assigned") }
            );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);

            foreach (var otherInfo in order.Items.SelectMany(i => i.Address.OtherInfo))
            {
                Assert.IsTrue(otherInfo.Contains("_assigned"));
            }
        }

        [Test]
        public void ActionBusinessRule_With_CollectionFilter_Should_Update_Only_Filtered_Items()
        {
            var orderItems = CreateOrderItemsCollection(2, "Item")
                .Select((oi, idx) =>
                {
                    oi.Address = CreateAddress(country: idx == 0 ? "Canada" : "USA", state: "FL");
                    return oi;
                });

            var order = CreateOrder(orderItems);

            Assert.AreEqual("Canada", order.Items.First().Address.Country);
            Assert.AreEqual("USA", order.Items.Last().Address.Country);

            var rule = CreateActionRule(
                expression: "Items.Filter(Address.Country.Equals(@0), Address.Country.Assign(@1))",
                arguments: new[]
                    {
                        new RuleArgument("Canada"),
                        new RuleArgument("USA")
                    }
                );

            var compiledActionRule = ActionRuleCompilerWithLogger.Compile<Order>(rule);
            compiledActionRule.Compiled(order);

            Assert.AreEqual(2, order.Items.Count());

            foreach (var country in order.Items.Select(i => i.Address.Country))
            {
                Assert.AreEqual("USA", country);
            }
        }
    }
}
