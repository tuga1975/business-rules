using System;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.EntityTracking
{
    public class EntityChangeInformation : IEntityChangeInformation
    {
        public int IdRule { get; set; }
        public string RuleName { get; set; }
        public string Operation { get; set; }
        public string Member { get; set; }
        public IEnumerable<object> Values { get; set; } = new List<string>();
        public IEntityChangeInformation Filter { get; set; }
        public IEnumerable<int> EvaluationPath { get; set; } = new HashSet<int>();

        public override bool Equals(object obj)
        {
            var entityChangeContainer = obj as EntityChangeInformation;
            return entityChangeContainer != null && Operation.Equals(entityChangeContainer.Operation) && Member.Equals(entityChangeContainer.Member);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Operation?.GetHashCode() ?? 0);
                hash = hash * 23 + (Member?.GetHashCode() ?? 0);
                return hash;
            }
        }

        private string ValueToString(object value)
        {
            if (value is string valueStr)
                return $"'{ value }'";

            return value?.ToString() ?? "";
        }

        public override string ToString()
        {
            var values = string.Join(", ", Values.Select(ValueToString));
            var toString = $"Member: { Member }, Operation: { Operation.Replace("()", $"({ values })") }";

            if (Filter != null)
                toString += $", Filter: [{ Filter.ToString() }]";

            return toString;
        }
    }
}
