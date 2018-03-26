using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using System;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using SellerCloud.BusinessRules.Extensions.Helpers;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace SellerCloud.BusinessRules.Logging
{
    public class FileExpressionLogger : ILogger
    {
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        static FileExpressionLogger()
        {
            try
            {
                CleanUp();
            }
            catch (Exception)
            {
                Debug.WriteLine("Unable to clean log file");
            }
        }

        private static void CleanUp()
        {
            var app = Logger.Logger.Repository.GetAppenders()
                .FirstOrDefault(a => a.GetType() == typeof(log4net.Appender.RollingFileAppender));

            if (app != null)
            {
                var appender = app as log4net.Appender.RollingFileAppender;
                appender.ImmediateFlush = true;
                appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
                appender.ActivateOptions();

                File.WriteAllText(appender.File, string.Empty);
            }
        }

        public virtual void Log(params object[] items)
        {
            if (items.Length == 0)
                return;

            string logMessage = string.Empty;

            if (items.First() is Expression expression)
            {
                StackTrace stackTrace = new StackTrace();
                var stackFrames = stackTrace.GetFrames()
                    .TakeWhile(f => f.GetMethod().DeclaringType != typeof(RuntimeMethodHandle));

                var renderedExpression = expression.Render();
                renderedExpression = $"\t{ renderedExpression.Replace("\t", "\t\t") }";

                logMessage += $"{ Environment.NewLine }Method CallStack: { Environment.NewLine }";
                logMessage += string.Join(
                    Environment.NewLine,
                    stackFrames.Select(f => $"\t{ f.GetMethod().DeclaringType.Name }.{ f.GetMethod().Name }")
                        .Concat(new[]
                        {
                            string.Empty,
                            renderedExpression,
                            string.Empty
                        })
                );
            }

            if (items.Length > 1)
            {
                if (items[1] is string trackedChangeInformation)
                {
                    logMessage += $"{ Environment.NewLine }\tChanged Property -> { trackedChangeInformation }{ Environment.NewLine }{ Environment.NewLine }";
                }
            }
            else
            {
                logMessage += Environment.NewLine + Environment.NewLine;
            }

            Logger.Info(logMessage);
        }

        public Task LogAsync(params object[] items) => Task.Factory.StartNew(itms => Log(itms as object[]), items);
    }
}
