using SellerCloud.BusinessRules.Extensions.ExtensionMethods;
using SellerCloud.BusinessRules.Extensions.Helpers;
using SellerCloud.BusinessRules.Logging;
using SellerCloud.BusinessRules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Compilers
{
    public abstract class RuleCompiler
    {
        protected readonly ILogger Logger;
        private Type customExtensionMethodsType;

        protected RuleCompiler(Type customExtensionMethodsType = null, ILogger logger = null)
        {
            this.Logger = logger;
            this.customExtensionMethodsType = customExtensionMethodsType;
        }

        //public static void SetCustomExtensionMethodsType(Type customExtensionMethodsType) => 
        //    RuleCompiler.customExtensionMethodsType = customExtensionMethodsType;

        protected void LogExpression(Expression lambda)
        {
            Logger?.Log(lambda);
        }

        protected Task LogExpressionAsync(Expression lambda) => Logger?.LogAsync(lambda);

        private bool MemberIsValue(string member) => member.StartsWith("@");
        private bool MemberIsMethod(string member) => Regex.Match(member, @"[^\(]+\(.*\)", RegexOptions.IgnoreCase).Success;
        
        private IEnumerable<MethodInfo> GetCollectionMethods() => 
            typeof(CollectionMethods).GetStaticMethods()
                .Concat(
                    GetCustomExtensionMethods()
                        .Where(m => m.GetParameters().First().ParameterType.IsCollection())
                );
        private IEnumerable<MethodInfo> GetCustomExtensionMethods() => customExtensionMethodsType?.GetStaticMethods() ?? Enumerable.Empty<MethodInfo>();
        private IEnumerable<MethodInfo> GetBusinessRulesMethods() => typeof(BooleanMethods).GetStaticMethods().Concat(typeof(ActionMethods).GetStaticMethods());

        private MethodInfo GetMethod(Type type, string methodName, IEnumerable<MethodInfo> methods, IEnumerable<Expression> arguments)
        {
            var genericType = type.IsGenericType ? type.GenericTypeArguments.First() : type;

            var filteredMethods = methods.Where(m => m.Name == methodName)
                    .Where(m =>
                    {
                        var parameterType = m.GetParameters().First().ParameterType;
                        return (parameterType.IsCollection() && type.IsCollection()) || parameterType.IsGenericParameter || parameterType == genericType;
                    });

            return filteredMethods.FirstOrDefault(m => FindMethod(m, arguments));
        }

        private MethodInfo GetMethod(Type type, string methodName, IEnumerable<Expression> arguments)
        {
            if (type.IsCollection())
                return GetMethod(type, methodName, GetCollectionMethods(), arguments);

            return GetMethod(type, methodName, GetCustomExtensionMethods(), arguments)
                ?? GetMethod(type, methodName, GetBusinessRulesMethods(), arguments);
        }

        private bool FindMethod(MethodInfo method, IEnumerable<Expression> arguments)
        {
            var methodParmeters = method.GetParameters().Skip(method.IsExtensionMethod() ? 1 : 0);
            var parametersTypeMatch = MethodParametersTypeMatch(arguments, methodParmeters);
            return parametersTypeMatch.All(result => result) && Enumerable.Count(methodParmeters) == Enumerable.Count(arguments);
        }

        private IEnumerable<bool> MethodParametersTypeMatch(IEnumerable<Expression> arguments, IEnumerable<ParameterInfo> methodParmeters) => 
            methodParmeters.Select((p, i) =>
            {
                var arg = arguments.ElementAt(i);
                if (p.ParameterType.IsFunction())
                {
                    if (arg.NodeType == ExpressionType.Call)
                    {
                        var callExpression = (MethodCallExpression)arg;
                        return callExpression.Arguments.First().Type == callExpression.Type && Enumerable.Count(p.ParameterType.GenericTypeArguments.Distinct()) == 1
                            || arg.Type == p.ParameterType.GenericTypeArguments.Last();
                    }

                    if (arg.Type == p.ParameterType.GenericTypeArguments.Last())
                        return true;
                }

                if (p.ParameterType.IsAction() && arg.NodeType == ExpressionType.Call)
                    return true;

                return !p.ParameterType.IsFunction() && !p.ParameterType.IsAction() && arg.NodeType != ExpressionType.Call;
            });

        private Expression ConstructExpression(ParameterExpression param, IRule rule, string member)
        {
            var ruleCopy = (IRule)rule.Clone();
            ruleCopy.Expression = member;
            
            var result = Construct(param.Type, ruleCopy, param);
            return result;
        }

        private Expression MakeLambda(Type type, ParameterExpression param, Expression expression, bool createAssingExpression)
        {
            var types = new List<Type>();
            var genericArguments = type.GetGenericArguments();

            types.Add(param.Type);

            if (Enumerable.Count(genericArguments) == 2)
            {
                types.Add(genericArguments.Last());
            }

            var genericTypes = types
                .Select(a => a.IsGenericParameter ? (createAssingExpression ? param.Type : expression.Type) : a)
                .ToArray();

            var lambdaType = CreateLambdaType(type, param.Type, expression.Type, genericTypes);

            if (createAssingExpression)
            {
                expression = MakeAssignExpression(expression);
                expression = Expression.Block(expression, param);
            }

            return Expression.Lambda(lambdaType, expression, param);
        }

        private Type CreateLambdaType(Type type, Type paramType, Type expressionType, IEnumerable<Type> genericTypes)
        {
            if (type.IsFunction())
                return typeof(Func<,>).MakeGenericType(genericTypes.ToArray());
            else if (type.IsAction())
                return typeof(Action<>).MakeGenericType(genericTypes.ToArray());

            return typeof(Func<,>).MakeGenericType(paramType, expressionType);
        }

        private Expression MakeConvertExpression(Expression expression, Type type)
        {
            if (type.IsCollection())
            {
                if (!type.IsGenericType && expression.NodeType == ExpressionType.Constant)
                {
                    var constantExpression = (ConstantExpression)expression;
                    var newCollectionType = type.GetCollectionElementType();
                    var currentCollectionType = constantExpression.Type.GetCollectionElementType();

                    var convertEnumerableMethod = this.GetType().GetMethod("ConvertEnumerable").MakeGenericMethod(currentCollectionType, newCollectionType);
                    var convertedCollection = convertEnumerableMethod.Invoke(this, new object[] { constantExpression.Value });
                    return Expression.Constant(convertedCollection);
                }

                return expression;
            }

            try
            {
                return Expression.Convert(expression, type);
            }
            catch (Exception)
            {
                return expression;
            }
        }

        private IEnumerable<TNew> ConvertEnumerable<TCurrent, TNew>(IEnumerable<TCurrent> collection)
        {
            var type = typeof(TNew);
            foreach (var item in collection)
                yield return (TNew)Convert.ChangeType(item, type);
        }

        private Expression CreateMethodCallExpression(Type type, string member, IRule rule, Expression expression)
        {
            var idx = member.IndexOf("(");
            var methodName = member.Substring(0, idx++);
            var methodArguments = member.Substring(idx, member.LastIndexOf(")") - idx)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Trim());

            var arguments = CreateExpressionArguments(type, rule, methodArguments);

            var method = GetMethod(type, methodName, arguments.Select(a => a.Item2));

            var methodParams = GetExpressionMethodParameters(method, expression, methodArguments, arguments).ToArray();

            if (method.IsGenericMethod)
            {
                var genericMethodTypes = GetGenericMethodTypes(method, expression, methodParams).ToArray();
                method = method.MakeGenericMethod(genericMethodTypes);
            }

            return MakeMethodCallExpression(method, new[] { expression }.Concat(methodParams));
        }

        private IEnumerable<Type> GetGenericMethodTypes(MethodInfo method, Expression expression, Expression[] methodParams)
        {
            var genericType = expression.Type.IsGenericType ? expression.Type.GenericTypeArguments.First() : expression.Type;

            return (new[] { genericType })
                .Concat(
                    method.GetGenericArguments()
                        .Skip(1)
                        .Select((a, i) =>
                        {
                            var methodParamType = methodParams.ElementAt(i).Type;
                            return methodParamType.IsFunction() ? methodParamType.GenericTypeArguments.Last() : methodParamType;
                        })
                ).ToArray();
        }

        private IEnumerable<Tuple<ParameterExpression, Expression>> CreateExpressionArguments(Type type, IRule rule, IEnumerable<string> methodArguments) =>
            methodArguments.Select(a =>
            {
                var targetType = type.IsGenericType ? type.GenericTypeArguments.First() : type;
                var param = Expression.Parameter(targetType);

                var expr = MemberIsValue(a) ? MakeConstantExpression(rule, a) : ConstructExpression(param, rule, a);
                return new Tuple<ParameterExpression, Expression>(param, expr);
            });

        private IEnumerable<Expression> GetExpressionMethodParameters(MethodInfo method, Expression expression, IEnumerable<string> methodArguments, IEnumerable<Tuple<ParameterExpression, Expression>> arguments) =>
            method.GetParameters()
                .Skip(method.IsExtensionMethod() ? 1 : 0)
                .Select((p, i) =>
                {
                    var methodArg = methodArguments.ElementAt(i);
                    var arg = arguments.ElementAt(i);
                    var parameterType = p.ParameterType.IsGenericParameter ? expression.Type.GetGenericType() : p.ParameterType;

                    if (methodArg.StartsWith("@"))
                    {
                        return MakeConvertExpression(arg.Item2, parameterType);
                    }

                    var expressionInfoAttribute = p.GetCustomAttribute<ExpressionInfoAttribute>() ?? method.GetCustomAttribute<ExpressionInfoAttribute>();

                    return MakeLambda(
                        type: parameterType,
                        param: arg.Item1,
                        expression: arg.Item2,
                        createAssingExpression: expressionInfoAttribute?.ExpressionType == ExpressionType.Assign
                    );
                });

        private Expression ConvertExpression(Type targetType, Expression expression)
        {
            if (targetType == expression.Type || expression.NodeType != ExpressionType.Constant)
                return expression;

            var constantExpression = (ConstantExpression)expression;
            
            if (targetType.IsCollection() && constantExpression.Type.IsCollection())
            {
                var targetTypeCollectionElementType = targetType.GetCollectionElementType();
                var collectionElementType = constantExpression.Type.GetCollectionElementType();

                if (targetTypeCollectionElementType != collectionElementType)
                {
                    var convertMethod = typeof(CollectionHelper).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public);

                    convertMethod = convertMethod.MakeGenericMethod(collectionElementType, targetTypeCollectionElementType);

                    var convertedCollection = convertMethod.Invoke(null, new [] { constantExpression.Value });
                    return Expression.Constant(convertedCollection);
                }
            }

            return Expression.Convert(expression, targetType);
        }

        private Expression MakeMethodCallExpression(MethodInfo method, IEnumerable<Expression> arguments)
        {
            var methodParameters = method.GetParameters();
            arguments = arguments.Select((a, i) => ConvertExpression(methodParameters.ElementAt(i).ParameterType, a));

            return Expression.Call(method, arguments);
        }

        private Expression MakeConstantExpression(IRule rule, string methodArgument)
        {
            var argIndex = int.Parse(methodArgument.Remove(0, 1));
            var arg = rule.Arguments.ElementAt(argIndex).Value;
            return Expression.Constant(arg);
        }

        protected Expression MakeAssignExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Call)
            {
                var assignableExpression = ((MethodCallExpression)expression).Arguments.First();
                if (expression.Type != typeof(void) && (assignableExpression.NodeType == ExpressionType.MemberAccess || assignableExpression.NodeType == ExpressionType.Parameter))
                    return Expression.Assign(assignableExpression, expression);
            }

            return expression;
        }

        protected Expression Construct<T>(IRule rule, Expression expression)
        {
            return Construct(typeof(T), rule, expression);
        }

        private int FindClosingBrackedIndex(string expression)
        {
            var openBracketsCount = 0;
            var matches = Regex.Matches(expression, @"\(|\)");
            foreach (Match match in matches)
            {
                if (match.Value == "(")
                    openBracketsCount++;
                else
                    openBracketsCount--;

                if (openBracketsCount == 0)
                    return match.Index;
            }

            return 0;
        }

        protected Expression Construct(Type type, IRule rule, Expression expression)
        {
            int idx;
            var expr = rule.Expression;

            do
            {
                idx = expr.IndexOf('.');
                string member = expr.Trim();

                if (idx != -1)
                {
                    var openingBracketIdx = expr.IndexOf("(");
                    if (openingBracketIdx < idx)
                    {
                        var closingBranckedIdx = FindClosingBrackedIndex(expr);
                        idx = expr.IndexOf('.', closingBranckedIdx);
                    }

                    if (idx != -1)
                    {
                        member = expr.Substring(0, idx).Trim();
                        expr = expr.Substring(idx + 1);
                    }
                }

                expression = CreateExpression(type, rule, expression, member);
                type = expression.Type;
            }
            while (idx != -1);

            return expression;
        }

        private Expression CreateExpression(Type type, IRule rule, Expression expression, string member)
        {
            if (MemberIsMethod(member))
                return CreateMethodCallExpression(type, member, rule, expression);

            var property = type.GetProperty(member, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
                return Expression.Property(expression, property);

            throw new ArgumentNullException(member, "Target member not found");
        }
    }
}
