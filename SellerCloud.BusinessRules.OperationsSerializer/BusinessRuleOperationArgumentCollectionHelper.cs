using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public static class BusinessRuleOperationArgumentCollectionHelper
    {
        public static IEnumerable<BusinessRuleOperationArgument> MergeParameters(this IEnumerable<BusinessRuleOperationArgument> source, IEnumerable<ParameterInfo> parameters)
        {
            return source.Select(arg =>
            {
                var param = parameters.ElementAt(arg.Position);
                arg.ApplicableTypes = arg.ApplicableTypes.Concat(new[] { param.ParameterType });
                return arg;
            });
        }
    }
}
