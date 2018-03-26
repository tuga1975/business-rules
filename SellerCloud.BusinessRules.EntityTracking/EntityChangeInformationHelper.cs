using SellerCloud.BusinessRules.Shared;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.EntityTracking
{
    public static class EntityChangeInformationHelper
    {
        public static IEnumerable<IEntityChangeInformation> Add(this IEnumerable<IEntityChangeInformation> source, params IEntityChangeInformation[] entityChangeInformation)
        {
            var list = source.ToList();

            if (entityChangeInformation != null)
            {
                list.AddRange(entityChangeInformation);
            }

            return list.AsEnumerable();
        }
    }
}