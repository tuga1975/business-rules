using SellerCloud.BusinessRules.EntityTracking;
using System;

namespace SellerCloud.BusinessRules.Compilers
{
    public class CompiledActionRule<T> : ICompiledActionRule<T>
    {
        public Action<T> Compiled { get; set; }

        public IEntityChangeInformation EntityChangeInformation { get; set; }

        public CompiledActionRule(Action<T> compiled, IEntityChangeInformation entityChangeInformation)
        {
            Compiled = compiled;
            EntityChangeInformation = entityChangeInformation;
        }
    }
}
