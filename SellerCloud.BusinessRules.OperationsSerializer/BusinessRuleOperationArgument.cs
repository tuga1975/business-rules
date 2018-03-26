using System;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public class BusinessRuleOperationArgument
    {
        public int Position { get; set; }
        public IEnumerable<Type> ApplicableTypes { get; set; } = new HashSet<Type>();

        private string ApplicableTypesToString() => string.Join("; ", ApplicableTypes.Select(t => t.AssemblyQualifiedName ?? t.FullName ?? t.Name));

        public override string ToString() => $"[{ Position }]->{ ApplicableTypesToString() }";
    }
}
