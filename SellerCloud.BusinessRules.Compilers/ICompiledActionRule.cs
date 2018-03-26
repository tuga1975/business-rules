using SellerCloud.BusinessRules.EntityTracking;
using System;

namespace SellerCloud.BusinessRules.Compilers
{
    public interface ICompiledActionRule<T>
    {
        Action<T> Compiled { get; set; }

        IEntityChangeInformation EntityChangeInformation { get; set; }
    }
}
