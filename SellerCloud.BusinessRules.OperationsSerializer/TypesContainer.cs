using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.OperationsSerializer
{
    public static class TypesContainer
    {
        public static IEnumerable<Type> ImplementedSystemTypes { get; private set; }

        static TypesContainer()
        {
            ImplementedSystemTypes = new[]
                {
                    typeof(int),
                    typeof(long),
                    typeof(double),
                    typeof(decimal),
                    //typeof(string),
                    typeof(DateTime),
                };
        }
    }
}
