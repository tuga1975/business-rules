using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Attributes
{
    public class BusinessRuleApplicabilityScopeAttribute : BusinessRuleAttribute
    {
        public IEnumerable<Type> ApplicableTypes { get; set; }
        public string Name { get; set; }
        public bool Ignore { get; set; }

        public BusinessRuleApplicabilityScopeAttribute(params Type[] applicableTypes) => ApplicableTypes = applicableTypes;

        public BusinessRuleApplicabilityScopeAttribute(bool ignore, params Type[] applicableTypes) 
            : this(applicableTypes) => Ignore = ignore;

        public BusinessRuleApplicabilityScopeAttribute(string name, params Type[] applicableTypes) 
            : this(applicableTypes) => Name = name;

        public BusinessRuleApplicabilityScopeAttribute(bool ignore, string name, params Type[] applicableTypes) 
            : this(name, applicableTypes) => Ignore = ignore;
    }
}
