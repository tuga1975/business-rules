using SellerCloud.BusinessRules.Attributes;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Models
{
    public class OrderItem : Entity
    {
        [BusinessRuleProperty]
        public string Name { get; set; }

        [BusinessRuleProperty]
        public Address Address { get; set; }
    }
}
