using System;
using System.Configuration;
using System.Data.SqlClient;
using Serilog;
using Serilog.Events;

namespace Flogging.Core
{
    public static class Flogger
    {
        private static readonly ILogger PerfLogger;
        private static readonly ILogger UsageLogger;
        private static readonly ILogger ErrorLogger;
        private static readonly ILogger DiagnosticLogger;

        static Flogger()
        {
            PerfLogger = new LoggerConfiguration()
                .WriteTo.File("Logger/perf.txt")
                .CreateLogger();
            UsageLogger = new LoggerConfiguration()
                .WriteTo.File("Logger/usage.txt")
                .CreateLogger();
            ErrorLogger = new LoggerConfiguration()
                .WriteTo.File("Logger/error.txt")
                .CreateLogger();
            DiagnosticLogger = new LoggerConfiguration()
                .WriteTo.File("Logger/diagnostic.txt")
                .CreateLogger();
        }

        public static void WritePerf(FlogDetail infoToLog)
        {
            PerfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteUsage(FlogDetail infoToLog)
        {
            UsageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteError(FlogDetail infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }

            ErrorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        private static string FindProcName(Exception exception)
        {
            if (exception is SqlException sqlEx)
            {
                var procName = sqlEx.Procedure;
                if (!string.IsNullOrEmpty(procName))
                {
                    return procName;
                }
            }

            if (!string.IsNullOrEmpty((string) exception.Data["Procedure"]))
            {
                return (string) exception.Data["Procedure"];
            }

            if (exception.InnerException != null)
            {
                return FindProcName(exception.InnerException);
            }

            return null;
        }

        public static void WriteDiagnostic(FlogDetail infoToLog)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
            if (!writeDiagnostics)
            {
                return;
            }

            DiagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        private static string GetMessageFromException(Exception exception)
        {
            if (exception.InnerException != null)
            {
                return GetMessageFromException(exception.InnerException);
            }

            return exception.Message;
        }
    }
}
