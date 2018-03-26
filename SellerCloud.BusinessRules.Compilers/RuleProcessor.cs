using SellerCloud.BusinessRules.EntityTracking;
using SellerCloud.BusinessRules.Rules;
using SellerCloud.BusinessRules.Rules.RuleModule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.Compilers
{
    public class RuleProcessor : IRuleProcessor
    {
        private readonly IBooleanRuleCompiler _booleanRuleCompiler;
        private readonly IActionRuleCompiler _actionRuleCompiler;
        private HashSet<int> _evaluationPath = new HashSet<int>();
        private HashSet<IEnumerable<int>> _evaluationPathPerRuleModule = new HashSet<IEnumerable<int>>();

        public RuleProcessor(IBooleanRuleCompiler booleanRuleCompiler, IActionRuleCompiler actionRuleCompiler)
        {
            this._booleanRuleCompiler = booleanRuleCompiler;
            this._actionRuleCompiler = actionRuleCompiler;
        }

        public IRuleProcessorResult<T> ProcessMany<T>(IEnumerable<RuleModule> modules, T entity)
        {
            var entitiesChangeInformationSet = new HashSet<IEntityChangeInformation>();
            IRuleProcessorResult<T> result = new RuleProcessorResult<T>(entity);

            foreach (var module in modules)
            {
                this._evaluationPath = new HashSet<int>();
                result = Process(module, entity, entitiesChangeInformationSet);
            }

            return new RuleProcessorResult<T>(result.Entity, result.EntitiesChangeInformation);
        }

        public IRuleProcessorResult<T> Process<T>(RuleModule module, T entity, HashSet<IEntityChangeInformation> entitiesChangeInformationSet = null, bool applyActions = true)
        {
            entitiesChangeInformationSet = entitiesChangeInformationSet ?? new HashSet<IEntityChangeInformation>();
            var resultEntity = ProcessEntity(module, entity, entitiesChangeInformationSet, applyActions);
            return new RuleProcessorResult<T>(resultEntity, _evaluationPath, entitiesChangeInformationSet);
        }

        private RuleModule GetNextModule<T>(RuleModule module, T entity)
        {
            var evalResult = EvalBooleanRuleModule(module, entity);
            var nextModule = evalResult ? module.IfTrue : module.IfFalse;
            return nextModule;
        }

        private T ProcessEntity<T>(RuleModule module, T entity, HashSet<IEntityChangeInformation> entityChangeInformationSet, bool applyActions)
        {
            if (module != null)
            {
                _evaluationPath?.Add(module.Id);

                if (module.RuleModuleType == RuleType.Boolean)
                {
                    var nextModule = GetNextModule(module, entity);
                    return ProcessEntity(nextModule, entity, entityChangeInformationSet, applyActions);
                }
                else
                {
                    ProcessActionRuleModule(module, entity, entityChangeInformationSet, applyActions);
                }
            }

            return entity;
        }

        private Func<IRuleCompilable, bool> EvalBooleanRule<T>(T entity) =>
            rule =>
            {
                var compiled = this._booleanRuleCompiler.Compile<T>(rule as IRule);
                return compiled(entity);
            };


        private bool EvalBooleanRuleModule<T>(RuleModule ruleModule, T entity)
        {
            if (ruleModule.BooleanEvalLogic.Value == BooleanRuleModuleEvalLogic.And)
            {
                return ruleModule.Rules.All(EvalBooleanRule(entity));
            }

            return ruleModule.Rules.Any(EvalBooleanRule(entity));
        }

        private void ProcessActionRuleModule<T>(RuleModule ruleModule, T entity, HashSet<IEntityChangeInformation> entityChangeInformationSet, bool applyActions)
        {
            foreach (var rule in ruleModule.Rules)
            {
                if (rule.Type == RuleCompilableType.Rule)
                {
                    var compiledActionRuleResult = this._actionRuleCompiler.Compile<T>(rule as IRule);

                    if (this._evaluationPath != null)
                    {
                        compiledActionRuleResult.EntityChangeInformation.EvaluationPath = this._evaluationPath;
                    }
                    
                    entityChangeInformationSet.Add(compiledActionRuleResult.EntityChangeInformation);

                    if (applyActions)
                    {
                        compiledActionRuleResult.Compiled(entity);
                    }
                }
                else
                {
                    ProcessEntity(rule as RuleModule, entity, entityChangeInformationSet, applyActions);
                }
            }
        }
    }
}