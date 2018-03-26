using SellerCloud.BusinessRules.Rules;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Compilers
{
    public interface IActionRuleCompiler
    {
        ICompiledActionRule<T> Compile<T>(IRule rule, bool trackChanges = true);

        Task<ICompiledActionRule<T>> CompileAsync<T>(IRule rule, bool trackChanges = true);
    }
}
