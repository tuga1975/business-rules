using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using SellerCloud.BusinessRules.Extensions.Helpers;

namespace SellerCloud.BusinessRules.Logging
{
    public class DebugExpressionLogger : ILogger
    {
        public void Log(params object[] items)
        {
            if (items.Length == 0)
                return;

            var expression = items.First() as Expression;
            if (expression != null)
            {
                Debug.WriteLine($"{ Environment.NewLine }{ expression.Render() }{ Environment.NewLine }");
            }
        }

        public Task LogAsync(params object[] items) => Task.Factory.StartNew(itms => Log(itms as object[]), items);
    }
}
