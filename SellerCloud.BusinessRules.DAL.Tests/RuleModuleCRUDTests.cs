using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SellerCloud.BusinessRules.Rules;
using SellerCloud.BusinessRules.Rules.RuleModule;

namespace SellerCloud.BusinessRules.DAL.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class RuleModuleCRUDTests : BusinessRulesDALTestBase
    {
        [Test]
        public void RuleModule_Create()
        {
            var ruleModule = new Rules.RuleModule.RuleModule
            {
                Name = "Test rule module",
                ClientID = this.ClientId
            };

            var id = this.RuleModuleService.CreateAsync(ruleModule).Result;
            Assert.AreNotEqual(0, id);

            var createdRuleModule = this.RuleModuleService.InquireAsync(id).Result;
            Assert.AreEqual(ruleModule.Name, createdRuleModule.Name);

            var list = this.RuleModuleService.GetAllAsync().Result;
            Assert.IsTrue(list.Any());
        }

        [Test]
        public void RuleModule_Create_With_Parent()
        {
            var parentRuleModule = new Rules.RuleModule.RuleModule
            {
                Name = "Parent Rule Module",
                ClientID = this.ClientId
            };

            var ruleModule = new Rules.RuleModule.RuleModule
            {
                Name = "Child Rule Module",
                ParentRuleModule = parentRuleModule,
                ClientID = this.ClientId
            };

            var id = this.RuleModuleService.CreateAsync(ruleModule).Result;
            Assert.AreNotEqual(0, id);

            var createdRuleModule = this.RuleModuleService.InquireAsync(id).Result;
            Assert.AreEqual(ruleModule.Name, createdRuleModule.Name);
            
        }

        [Test]
        public void RuleModule_Create_With_LinkedModules()
        {
            var ruleModule = new Rules.RuleModule.RuleModule
            {
                Name = "Rule Module",
                ClientID = this.ClientId,
                EnabledFlag = true
            };

            var ifTrueRuleModule = new Rules.RuleModule.RuleModule
            {
                Name = "If Yes",
                ParentRuleModule = ruleModule,
                RootRuleModule = ruleModule,
                PathType = RuleModulePath.IfTrue,
                ClientID = this.ClientId
            };

            var id = this.RuleModuleService.CreateAsync(ifTrueRuleModule).Result;
            Assert.AreNotEqual(0, id);

            var rootModules = this.RuleModuleService.GetAllRootWithLinkedModules(this.ClientId).Result;
            Assert.IsTrue(rootModules.Any());
            Assert.IsNotNull(rootModules.First().IfTrue);
            Assert.AreEqual(ifTrueRuleModule.Name, rootModules.First().IfTrue.Name);

            var rootModule = rootModules.First(rm => rm.Id == ruleModule.Id);

            var createdRuleModule = this.RuleModuleService.InquireWithLinkedModules(rootModule.Id).Result;
            Assert.AreEqual(ruleModule.Name, createdRuleModule.Name);
            Assert.IsNotNull(createdRuleModule.IfTrue);
            Assert.AreEqual(ifTrueRuleModule.Name, createdRuleModule.IfTrue.Name);
        }

        [Test]
        public void RuleModule_List_Root()
        {
            var ruleModules = Enumerable.Range(0, 10).Select(i => new Rules.RuleModule.RuleModule
            {
                Name = $"Module_{i}",
                ClientID = this.ClientId
            });

            var ids = this.RuleModuleService.CreateRangeAsync(ruleModules).Result;
            Assert.AreEqual(ruleModules.Count(), ids.Count());

            var rootRuleModules = this.RuleModuleService.GetAllRoot(this.ClientId).Result
                .Where(rm => ids.Contains(rm.Id));
            Assert.AreEqual(ruleModules.Count(), rootRuleModules.Count());

        }

        [Test]
        public void RuleModule_Create_With_Rules()
        {
            var ruleModule = new Rules.RuleModule.RuleModule
            {
                Name = "Test rule module",
                ClientID = this.ClientId,
                Rules = new HashSet<IRuleCompilable>
                {
                    new Rules.Rule
                    {
                        Name = "Test rule",
                        SaveArguments = new List<Rules.RuleArgument>
                        {
                            new Rules.RuleArgument(20)
                            {
                                Position = 0
                            }
                        },
                        RuleType = RuleType.Boolean,
                        Expression = "Order.ID.Equals(@0)"
                    }
                }
            };

            var id = this.RuleModuleService.CreateAsync(ruleModule).Result;
            Assert.AreNotEqual(0, id);

            var createdRuleModule = this.RuleModuleService.InquireAsync(id).Result;
            Assert.AreEqual(ruleModule.Name, createdRuleModule.Name);
            Assert.AreEqual(ruleModule.Rules.Count(), createdRuleModule.Rules.Count());
            Assert.AreEqual(((Rules.Rule)ruleModule.Rules.First()).Arguments.Count(), ((Rules.Rule)createdRuleModule.Rules.First()).Arguments.Count());
            Assert.AreEqual(((Rules.Rule)ruleModule.Rules.First()).Name, ((Rules.Rule)createdRuleModule.Rules.First()).Name);
        }
    }
}
