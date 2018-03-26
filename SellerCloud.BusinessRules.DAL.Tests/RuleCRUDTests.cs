using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SellerCloud.BusinessRules.Rules;

namespace SellerCloud.BusinessRules.DAL.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class RuleCRUDTests : BusinessRulesDALTestBase
    {
        private Rules.Rule CreateRule()
        {
            var rand = new System.Random();
            var rule = new Rules.Rule
            {
                Name = "Test rule",
                SaveArguments = Enumerable.Range(0, 5)
                    .Select(index => new Rules.RuleArgument
                    {
                        Position = rand.Next(0, 100),
                        SaveValue = new string[] { "Alabama1", "Alabama2" },
                        ValueType = typeof(string).FullName
                    })
                    .ToList(),
                RuleType = RuleType.Boolean,
                Expression = "Order.TrackingNumber.IsInList(@0)"
            };

            return rule;
        }

        private Rules.RuleModule.RuleModule CreateRuleModule(Rules.Rule rule)
        {
            var ruleModule = new Rules.RuleModule.RuleModule
            {
                Name = "Test rule module",
                Rules = new HashSet<IRuleCompilable>
                {
                    rule
                }
            };

            var id = this.RuleModuleService.CreateAsync(ruleModule).Result;
            var createdRuleModule = this.RuleModuleService.InquireAsync(id).Result;
            return createdRuleModule;
        }

        [Test]
        public void Rule_Create_With_Argument_Having_Array_as_Value()
        {
            var rule = CreateRule();
            var createdRuleModule = CreateRuleModule(rule);
            var createdRule = (Rules.Rule)createdRuleModule.Rules.First();
            Assert.AreEqual(rule.Name, createdRule.Name);
            Assert.AreEqual(rule.RuleType, createdRule.RuleType);
            CollectionAssert.AreEqual((ICollection)rule.Arguments.First().Value, (ICollection)createdRule.Arguments.First().Value);
        }

        [Test]
        public void Rule_Should_Be_Updated_Without_An_Error()
        {
            var rule = CreateRule();
            var createdRuleModule = CreateRuleModule(rule);
            var createdRule = (Rules.Rule)createdRuleModule.Rules.First();

            Assert.DoesNotThrow(() => this.RuleService.Update(createdRule), "Expected no exception, but got it fails :(");
        }

        [Test]
        public void Rule_Should_Be_Updated_When_SaveArguments_Is_Populated()
        {
            var rule = CreateRule();
            var createdRuleModule = CreateRuleModule(rule);
            var createdRule = (Rules.Rule)createdRuleModule.Rules.First();

            var currentRuleArgument = createdRule.Arguments.First();
            currentRuleArgument.SaveValue = currentRuleArgument.Value;
            currentRuleArgument.ValueType = typeof(string).FullName;

            createdRule.Name = "new rule name";
            createdRule.SaveArguments = createdRule.Arguments;

            this.RuleService.Update(createdRule);
            var updatedRule = this.RuleService.InquireWithArguments(createdRule.Id).Result;

            Assert.AreEqual(createdRule.Name, updatedRule.Name);
            Assert.AreEqual(createdRule.Arguments.Count, updatedRule.Arguments.Count);

            updatedRule.Name += " test";
            this.RuleService.Update(updatedRule);

            var secondUpdateRule = this.RuleService.InquireWithArguments(updatedRule.Id).Result;
            Assert.AreEqual(updatedRule.Name, secondUpdateRule.Name);
            Assert.AreEqual(updatedRule.Arguments.Count, secondUpdateRule.Arguments.Count);
        }
    }
}
