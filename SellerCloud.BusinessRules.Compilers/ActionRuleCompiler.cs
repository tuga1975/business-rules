using SellerCloud.BusinessRules.EntityTracking;
using SellerCloud.BusinessRules.Extensions.Helpers;
using SellerCloud.BusinessRules.Logging;
using SellerCloud.BusinessRules.Rules;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Compilers
{
    public class ActionRuleCompiler : RuleCompiler, IActionRuleCompiler
    {
        private readonly IEntityChangeTracker entityChangeTracker;

        protected void LogActionExpression(Expression lambda, string trackedChangeInformation)
        {
            Logger?.Log(lambda, trackedChangeInformation);
        }

        protected Task LogActionExpressionAsync(Expression lambda, string trackedChangeInformation) 
            => Logger?.LogAsync(lambda, trackedChangeInformation);

        public ActionRuleCompiler(IEntityChangeTracker entityChangeTracker, Type customExtensionMethodsType = null, ILogger logger = null) 
            : base(customExtensionMethodsType, logger)
        {
            this.entityChangeTracker = entityChangeTracker;
        }

        private IEntityChangeInformation GetEntityChangeInformation(IRule rule, LambdaExpression lambdaExpression, bool trackChanges = false)
        {
            if (trackChanges)
                return entityChangeTracker.TrackChanges(rule, lambdaExpression);

            return null;
        }

        public ICompiledActionRule<T> Compile<T>(IRule rule, bool trackChanges = true)
        {
            var lambdaExpression = CreateLambdaExpression<T>(rule);

            var entityChangeInformation = GetEntityChangeInformation(rule, lambdaExpression, trackChanges);

            LogActionExpression(lambdaExpression, entityChangeInformation?.ToString());

            var compiled = lambdaExpression.Compile();

            return new CompiledActionRule<T>(compiled, entityChangeInformation);
        }

        public async Task<ICompiledActionRule<T>> CompileAsync<T>(IRule rule, bool trackChanges = true)
        {
            var lambdaExpression = await CreateLambdaExpressionAsync<T>(rule);

            var entityChangeInformation = GetEntityChangeInformation(rule, lambdaExpression, trackChanges);

            await LogActionExpressionAsync(lambdaExpression, entityChangeInformation?.ToString());

            var compiled = lambdaExpression.Compile();

            return new CompiledActionRule<T>(compiled, entityChangeInformation);
        }

        public Expression<Action<T>> CreateLambdaExpression<T>(IRule rule)
        {
            var param = Expression.Parameter(typeof(T));
            var expression = Construct<T>(rule, param);

            var assignExpression = MakeAssignExpression(expression);

            var lambda = Expression.Lambda<Action<T>>(assignExpression, param);
            return lambda;
        }

        public Task<Expression<Action<T>>> CreateLambdaExpressionAsync<T>(IRule rule) =>
            Task.Factory.StartNew(r => CreateLambdaExpression<T>(r as IRule), rule);
    }
}
