using SellerCloud.BusinessRules.Attributes;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Models
{
    public class Address
    {
        [BusinessRuleProperty]
        public string Country { get; set; }

        [BusinessRuleProperty]
        public string State { get; set; }

        [BusinessRuleProperty]
        public string City { get; set; }

        [BusinessRuleProperty]
        public string Street { get; set; }

        [BusinessRuleProperty]
        public string Zip { get; set; }

        [BusinessRuleProperty(name: "Other Info")]
        public IEnumerable<string> OtherInfo { get; set; }

        [BusinessRuleProperty]
        public decimal Number { get; set; }

        [BusinessRuleProperty]
        public IEnumerable<Person> Tenantry { get; set; }
    }
}
