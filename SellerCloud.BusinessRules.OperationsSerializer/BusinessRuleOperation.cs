using SellerCloud.BusinessRules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public class BusinessRuleOperation
    {
        public string Operation { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<Type> ApplicableTypes { get; set; } = new List<Type>();
        public bool Ignore { get; set; }
        public IEnumerable<BusinessRuleOperationArgument> Arguments { get; set; } = new List<BusinessRuleOperationArgument>();
        public Type ReturnType { get; set; }
        public RuleType? RuleType { get; set; }
        public string ItemsEndpoint { get; set; }

        public override string ToString() => $"{ DisplayName }, Arguments: { string.Join(", ", Arguments.Select(a => a.ToString())) }";
    }
}
