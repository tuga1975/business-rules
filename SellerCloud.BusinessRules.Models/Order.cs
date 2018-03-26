using SellerCloud.BusinessRules.Attributes;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.Models
{
    public class Order : Entity
    {
        [BusinessRuleProperty]
        public string Label { get; set; }

        [BusinessRuleProperty]
        public double Amount { get; set; }

        [BusinessRuleProperty(name: "Shipping Status")]
        public OrderShippingStatus ShippingStatus { get; set; }

        [BusinessRuleProperty(name: "Shipping Address")]
        public Address ShippingAddress { get; set; }

        [BusinessRuleProperty(name: "From Address")]
        public Address FromAddress { get; set; }

        [BusinessRuleProperty]
        public IEnumerable<OrderItem> Items { get; set; }
    }
}
