using SellerCloud.BusinessRules.Extensions.ExtensionMethods;
using SellerCloud.BusinessRules.Rules;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SellerCloud.BusinessRules.EntityTracking
{
    public class EntityChangeTracker : IEntityChangeTracker
    {
        private IEntityChangeInformation ProcessConvertExpression(UnaryExpression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            return ProcessExpression(expression.Operand, entityChangeContainer, processedProperties);
        }

        private IEntityChangeInformation ProcessConstantExpression(ConstantExpression expression, IEntityChangeInformation entityChangeContainer)
        {
            entityChangeContainer.Values = entityChangeContainer.Values.Concat(new[] { expression.Value });
            return entityChangeContainer;
        }

        private IEntityChangeInformation ProcessMemberExpression(MemberExpression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            var member = expression.Member;
            var memberName = member.Name;
            var expressionToString = expression.ToString();

            if (processedProperties.Contains(expressionToString))
                return entityChangeContainer;

            if (expression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                entityChangeContainer = ProcessMemberExpression((MemberExpression)expression.Expression, entityChangeContainer, processedProperties);
            }

            if (!entityChangeContainer.Member.EndsWith("." + memberName))
            {
                entityChangeContainer.Member += "." + memberName;
            }

            processedProperties.Add(expressionToString);

            return entityChangeContainer;
        }

        private IEntityChangeInformation ProcessBinaryExpression(BinaryExpression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            entityChangeContainer.Operation = expression.NodeType.ToString();

            entityChangeContainer = ProcessExpression(expression.Left, entityChangeContainer, processedProperties);

            return ProcessExpression(expression.Right, entityChangeContainer, processedProperties);
        }
                
        private IEntityChangeInformation ProcessCallExpression(MethodCallExpression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            var expressionInfoAttribute = expression.Method.GetCustomAttribute<ExpressionInfoAttribute>();
            var skipArguments = 0;

            if (expressionInfoAttribute?.ExpressionPurposeType == ExpressionPurposeType.Filtering)
            {
                skipArguments = 2;

                entityChangeContainer.Filter = new EntityChangeInformation
                {
                    Member = entityChangeContainer.Member
                };

                var filterLambdaExpression = expression.Arguments.FirstOrDefault(arg => (arg as LambdaExpression)?.ReturnType == typeof(bool));
                entityChangeContainer.Filter = ProcessExpression(filterLambdaExpression, entityChangeContainer.Filter, new HashSet<string>());
            }

            entityChangeContainer.Operation = expression.Method.Name + "(";

            entityChangeContainer = expression.Arguments.Skip(skipArguments)
                .Aggregate(entityChangeContainer, (accumulate, arg) => ProcessExpression(arg, accumulate, processedProperties));

            if (!entityChangeContainer.Operation.EndsWith(")"))
                entityChangeContainer.Operation += ")";

            return entityChangeContainer;
        }

        private IEntityChangeInformation ProcessBlockExpression(BlockExpression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            return ProcessExpression(expression.Expressions.First(), entityChangeContainer, processedProperties);
        }

        private IEntityChangeInformation ProcessLambdaExpression(LambdaExpression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            return ProcessExpression(expression.Body, entityChangeContainer, processedProperties);
        }

        private IEntityChangeInformation ProcessExpression(Expression expression, IEntityChangeInformation entityChangeContainer, HashSet<string> processedProperties)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Assign:
                    return ProcessBinaryExpression((BinaryExpression)expression, entityChangeContainer, processedProperties);
                case ExpressionType.MemberAccess:
                    return ProcessMemberExpression((MemberExpression)expression, entityChangeContainer, processedProperties);
                case ExpressionType.Convert:
                    return ProcessConvertExpression((UnaryExpression)expression, entityChangeContainer, processedProperties);
                case ExpressionType.Constant:
                    return ProcessConstantExpression((ConstantExpression)expression, entityChangeContainer);
                case ExpressionType.Call:
                    return ProcessCallExpression((MethodCallExpression)expression, entityChangeContainer, processedProperties);
                case ExpressionType.Block:
                    return ProcessBlockExpression((BlockExpression)expression, entityChangeContainer, processedProperties);
                case ExpressionType.Lambda:
                    return ProcessLambdaExpression((LambdaExpression)expression, entityChangeContainer, processedProperties);
            }

            return entityChangeContainer;
        }

        public IEntityChangeInformation TrackChanges(IRule rule, LambdaExpression lambda)
        {
            var param = lambda.Parameters.First();

            var entityChangeContainer = new EntityChangeInformation
            {
                IdRule = rule.Id,
                RuleName = rule.Name,
                Member = param.Type.Name
            };

            var processedProperties = new HashSet<string>();

            return ProcessExpression(lambda.Body, entityChangeContainer, processedProperties);
        }
    }
}
