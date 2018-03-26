using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SellerCloud.BusinessRules.Extensions.Helpers
{
    internal class ExpressionTreeRenderer
    {
        public string Render(Expression expression)
        {
            var stringBuilder = new StringBuilder();

            ConstructExpression(expression, 0, "", stringBuilder);

            return stringBuilder.ToString();
        }

        private string RemoveLastOccurrence(string source, string target)
        {
            var lastIndex = source.LastIndexOf(target);
            if (lastIndex > -1)
                return source.Substring(0, lastIndex);

            return source;
        }

        private string GetBinaryOperator(BinaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    return " == ";
                case ExpressionType.NotEqual:
                    return " != ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.Add:
                    return " + ";
                case ExpressionType.Subtract:
                    return " - ";
                case ExpressionType.Divide:
                    return " / ";
                case ExpressionType.Multiply:
                    return " * ";
                case ExpressionType.Power:
                    return "^";
                case ExpressionType.Assign:
                    return " = ";
            }

            return string.Empty;
        }

        private void ConstructLambdaExpression(LambdaExpression lambdaExpression, int paramSuffix, string indentation, StringBuilder stringBuilder)
        {
            var parameters = string.Join(", ", lambdaExpression.Parameters.Select(p => p.ToFormattedString(paramSuffix)));
            parameters = lambdaExpression.Parameters.Count != 1 ? $"({ parameters })" : parameters;
            stringBuilder.Append($"{ indentation }{parameters} => { Environment.NewLine }");
            indentation += "\t";

            if (lambdaExpression.Body.NodeType != ExpressionType.Block)
            {
                stringBuilder.Append(indentation);
            }

            ConstructExpression(lambdaExpression.Body, paramSuffix, indentation, stringBuilder);
        }

        private void ConstructBinaryExpression(BinaryExpression expression, int paramSuffix, string indentation, StringBuilder stringBuilder)
        {
            var binaryOperator = GetBinaryOperator(expression);

            ConstructExpression(expression.Left, paramSuffix, indentation, stringBuilder);
            stringBuilder.Append(binaryOperator);
            ConstructExpression(expression.Right, paramSuffix, indentation, stringBuilder);
        }

        private void ConstructCallExpression(MethodCallExpression methodCallExpression, int paramSuffix, string indentation, StringBuilder stringBuilder)
        {
            var methodCallExpressionArgument = methodCallExpression.Object ?? methodCallExpression.Arguments.FirstOrDefault();
            ConstructExpression(methodCallExpressionArgument, paramSuffix, indentation, stringBuilder);
            stringBuilder.Append($".{ methodCallExpression.Method.Name }(");
            indentation += "\t";

            var methodArguments = methodCallExpression.Arguments.Skip(methodCallExpression.Object != null ? 0 : 1);
            var addIdentationOrNewLine = methodArguments.Any() && methodArguments.All(a => a.NodeType != ExpressionType.Constant && a.NodeType != ExpressionType.Convert);

            if (addIdentationOrNewLine)
            {
                stringBuilder.Append(Environment.NewLine);
            }

            if (methodArguments.Any())
            {
                paramSuffix++;

                var multipleArguments = methodArguments.Count() > 1;

                var paramsString = methodArguments
                    .Aggregate(new StringBuilder(), (current, next) =>
                    {
                        ConstructExpression(next, paramSuffix, indentation, current);
                        if (multipleArguments)
                        {
                            current.Append(", " + Environment.NewLine);
                        }
                        return current;
                    }, current => 
                    {
                        return current.ToString().TrimEnd().TrimEnd(',');
                    });

                stringBuilder.Append(paramsString);

                stringBuilder.Append($"{ (addIdentationOrNewLine ? (Environment.NewLine + RemoveLastOccurrence(indentation, "\t")) : "") })");
            }
            else
            {
                stringBuilder.Append(")");
            }
        }

        private void ConstructBlockExpression(Expression expression, int paramSuffix, string indentation, StringBuilder stringBuilder)
        {
            var blockExpression = (BlockExpression)expression;
            var lastParamExpression = blockExpression.Expressions.Last();

            stringBuilder.Append($"{ RemoveLastOccurrence(indentation, "\t") }{{{ Environment.NewLine }");

            if (lastParamExpression.NodeType == ExpressionType.Parameter)
            {
                blockExpression.Expressions.Take(blockExpression.Expressions.Count - 1)
                    .Aggregate(stringBuilder, (current, next) =>
                    {
                        current.Append(indentation);
                        ConstructExpression(next, paramSuffix, indentation, stringBuilder);
                        return current;
                    })
                    .Append($";{ Environment.NewLine }");

                stringBuilder.Append($"{ indentation }return { lastParamExpression.ToFormattedString(paramSuffix) };{ Environment.NewLine }").Append($"{ RemoveLastOccurrence(indentation, "\t") }}}");
            }
            else
            {
                blockExpression.Expressions.Aggregate(stringBuilder, (current, next) =>
                {
                    ConstructExpression(next, paramSuffix, indentation, stringBuilder);
                    current.Append($";{ Environment.NewLine }");
                    return current;
                }).Append($"}}");
            }
        }

        private void ConstructConvertExpression(UnaryExpression unaryExpression, int paramSuffix, string indentation, StringBuilder stringBuilder)
        {
            stringBuilder.Append("Convert(");
            ConstructExpression(unaryExpression.Operand, paramSuffix, indentation, stringBuilder);
            stringBuilder.Append(", " + unaryExpression.Type.FullName);
            stringBuilder.Append(")");
        }

        private void ConstructExpression(Expression expression, int paramSuffix, string indentation, StringBuilder stringBuilder)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    ConstructCallExpression((MethodCallExpression)expression, paramSuffix, indentation, stringBuilder);
                    return;
                case ExpressionType.Lambda:
                    ConstructLambdaExpression((LambdaExpression)expression, paramSuffix, indentation, stringBuilder);
                    return;
                case ExpressionType.Constant:
                    var constantExpression = (ConstantExpression)expression;
                    stringBuilder.Append(constantExpression.ToString());
                    return;
                case ExpressionType.Convert:
                    ConstructConvertExpression((UnaryExpression)expression, paramSuffix, indentation, stringBuilder);
                    return;
                case ExpressionType.MemberAccess:
                case ExpressionType.Parameter:
                    stringBuilder.Append(expression.ToFormattedString(paramSuffix));
                    return;
                case ExpressionType.Block:
                    ConstructBlockExpression(expression, paramSuffix, indentation, stringBuilder);
                    return;
            }

            if (expression is BinaryExpression binaryExpression)
            {
                ConstructBinaryExpression(binaryExpression, paramSuffix, indentation, stringBuilder);
            }
        }
    }
}
