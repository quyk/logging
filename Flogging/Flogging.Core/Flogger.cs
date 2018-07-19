using System;
using System.Configuration;
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
            ErrorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
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
    }
}