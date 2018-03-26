using SellerCloud.BusinessRules.Compilers;
using SellerCloud.BusinessRules.DAL.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Facade
{
    public class BusinessRulesFacade : IBusinessRulesFacade
    {
        private readonly IRuleProcessorFactory _ruleProcessorFactory;
        private readonly IRuleModuleService _ruleModuleService;
        private List<Rules.RuleModule.RuleModule> _ruleModules;
        private static ConcurrentDictionary<int, Rules.RuleModule.RuleModule> _allRuleModules;

        static BusinessRulesFacade()
        {
            _allRuleModules = new ConcurrentDictionary<int, Rules.RuleModule.RuleModule>();
        }

        public BusinessRulesFacade(IRuleProcessorFactory ruleProcessorFactory, IRuleModuleService ruleModuleService)
        {
            this._ruleProcessorFactory = ruleProcessorFactory;
            this._ruleModuleService = ruleModuleService;
        }

        private IRuleProcessor CreateRuleProcessor() =>
            this._ruleProcessorFactory.CreateRuleProcessor();

        public Rules.RuleModule.RuleModule GetRuleModule(int idRootRuleModule)
        {
            var ruleModule = _allRuleModules.GetOrAdd(idRootRuleModule, (idRuleModule) =>
            {
                return Task.Run(() => this._ruleModuleService.InquireWithLinkedModules(idRuleModule, true)).Result;
            });

            return ruleModule;
        }

        private void AddRuleModulesToDictionary(IEnumerable<Rules.RuleModule.RuleModule> ruleModules)
        {
            foreach (var ruleModule in ruleModules)
            {
                _allRuleModules.GetOrAdd(ruleModule.Id, ruleModule);
            }
        }

        public async Task LoadRuleModules(int clientId)
        {
            this._ruleModules = await this._ruleModuleService.GetAllRootWithLinkedModules(clientId);
            this.AddRuleModulesToDictionary(this._ruleModules);
        }

        public IRuleProcessorResult<T> Process<T>(Rules.RuleModule.RuleModule rootRuleModule, T entity) =>
            this.CreateRuleProcessor().Process(rootRuleModule, entity);

        public IRuleProcessorResult<T> ProcessAll<T>(T entity)
        {
            var filteredRuleModules = this._ruleModules
                .Where(rm =>
                {
                    var type = typeof(T);
                    var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    return properties
                        .Any(p => p.PropertyType.AssemblyQualifiedName == rm.ContextType && p.GetValue(entity) != null);
                });

            var ruleProcessor = this.CreateRuleProcessor();
            return ProcessAll(filteredRuleModules, entity, ruleProcessor);
        }

        public IRuleProcessorResult<T> ProcessAll<T>(IEnumerable<Rules.RuleModule.RuleModule> rootRuleModules, T entity, IRuleProcessor ruleProcessor = null)
        {
            ruleProcessor = ruleProcessor ?? this.CreateRuleProcessor();
            return ruleProcessor.ProcessMany(rootRuleModules, entity);
        }            

        public IEnumerable<IRuleProcessorResult<T>> ProcessAllEntities<T>(IEnumerable<T> entities)
        {
            var resultList = new ConcurrentBag<IRuleProcessorResult<T>>();
            var entitiesCount = entities.Count();

            ThreadPool.SetMinThreads(entitiesCount, entitiesCount);

            Parallel.ForEach(entities, entity =>
            {
                var result = this.ProcessAll(entity);
                resultList.Add(result);
            });

            return resultList;
        }
    }
}
