using System;
using System.Linq.Expressions;

namespace SellerCloud.BusinessRules.Extensions.ExtensionMethods
{
    public class ExpressionInfoAttribute : Attribute
    {
        public ExpressionType ExpressionType { get; set; }

        public ExpressionPurposeType ExpressionPurposeType { get; set; }

        public ExpressionInfoAttribute(ExpressionType expressionType)
        {
            ExpressionType = expressionType;
        }

        public ExpressionInfoAttribute(ExpressionPurposeType expressionPurposeType)
        {
            ExpressionPurposeType = expressionPurposeType;
        }
    }

}
