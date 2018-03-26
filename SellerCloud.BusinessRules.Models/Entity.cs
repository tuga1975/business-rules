using SellerCloud.BusinessRules.Attributes;

namespace SellerCloud.BusinessRules.Models
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
