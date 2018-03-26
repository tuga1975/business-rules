using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.Extensions.Helpers
{
    public static class CollectionHelper
    {
        public static IEnumerable<T2> Convert<T1, T2>(this IEnumerable<T1> source) => source.Select(i => (T2)System.Convert.ChangeType(i, typeof(T2)));        
    }
}
