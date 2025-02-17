using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace PetStore.Api.RegressionTests.Helpers
{
    static class LoggingHelper
    {
        // Logger instance for the class
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        // Initialize log4net (usually done at the start of the application)
        static LoggingHelper()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        // Log an Info message
        public static void LogInfo(string message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        // Log a Debug message
        public static void LogDebug(string message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        // Log an Error message
        public static void LogError(string message, Exception ex = null)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
            }
        }

        // Log a Fatal message
        public static void LogFatal(string message, Exception ex = null)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, ex);
            }
        }
    }
}