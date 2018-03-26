using System.Linq;

namespace SellerCloud.BusinessRules.Logging
{
    public class FileJsonLogger : FileExpressionLogger
    {
        public override void Log(params object[] items)
        {
            if (items.Length == 0)
                return;

            if (items.First() is string json)
            {
                Logger.Info(json);
            }
        }
    }
}
