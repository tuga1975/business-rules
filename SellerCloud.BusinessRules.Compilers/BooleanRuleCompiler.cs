using SellerCloud.BusinessRules.Logging;
using SellerCloud.BusinessRules.Rules;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Compilers
{
    public class BooleanRuleCompiler : RuleCompiler, IBooleanRuleCompiler
    {
        public BooleanRuleCompiler(Type customExtensionMethodsType = null, ILogger logger = null) : base(customExtensionMethodsType, logger)
        { }

        public Func<T, bool> Compile<T>(IRule rule)
        {
            var lambda = CreateLambdaExpression<T>(rule);

            LogExpression(lambda);

            return lambda.Compile();
        }

        public async Task<Func<T, bool>> CompileAsync<T>(IRule rule)
        {
            var lambda = await CreateLambdaExpressionAsync<T>(rule);

            await LogExpressionAsync(lambda);

            return lambda.Compile();
        }

        public Expression<Func<T, bool>> CreateLambdaExpression<T>(IRule rule)
        {
            var param = Expression.Parameter(typeof(T));
            var expression = Construct<T>(rule, param);

            var lambda = Expression.Lambda<Func<T, bool>>(expression, param);
            return lambda;
        }

        public Task<Expression<Func<T, bool>>> CreateLambdaExpressionAsync<T>(IRule rule) =>
            Task.Factory.StartNew(r => CreateLambdaExpression<T>(r as IRule), rule);
    }
}
