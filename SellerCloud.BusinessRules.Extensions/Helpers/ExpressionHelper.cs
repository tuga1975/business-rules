using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SellerCloud.BusinessRules.Extensions.Helpers
{
    public static class ExpressionHelper
    {
        public static string Render(this Expression source)
        {
            return new ExpressionTreeRenderer().Render(source);
        }

        public static string ToFormattedString(this ParameterExpression source, int suffix)
        {
            return Regex.Replace(source.ToString(), "Param_[0-9]+", $"Param_{ suffix }");
        }

        public static string ToFormattedString(this Expression source, int suffix)
        {
            if (source.NodeType == ExpressionType.Parameter || source.NodeType == ExpressionType.MemberAccess)
                return Regex.Replace(source.ToString(), "Param_[0-9]+", $"Param_{ suffix }");

            return source.ToString();
        }
    }
}
