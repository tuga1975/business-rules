using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.Logging
{
    public interface ILogger
    {
        void Log(params object[] items);

        Task LogAsync(params object[] items);
    }
}
