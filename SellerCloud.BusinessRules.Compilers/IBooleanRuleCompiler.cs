using SellerCloud.BusinessRules.Rules;
using System;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Compilers
{
    public interface IBooleanRuleCompiler
    {
        Func<T, bool> Compile<T>(IRule rule);

        Task<Func<T, bool>> CompileAsync<T>(IRule rule);
    }

}
