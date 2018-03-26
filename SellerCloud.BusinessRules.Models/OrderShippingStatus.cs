namespace SellerCloud.BusinessRules.Models
{
    public enum OrderShippingStatus
    { 
        Unknown = 0,
        Unshipped = 1,
        PartiallyShipped = 2,
        FullyShipped = 3,
        ReadyForPickup = 4,
    }
}
