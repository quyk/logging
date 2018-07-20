using System;
using Flogging.Core;

namespace Flogging.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fd = GetFlogDetail("starting apllication", null);
            Flogger.WriteDiagnostic(fd);

            var tracker = new PerfTracker("Flogger.Console_Execution", "", fd.UserName, fd.Location, fd.Product, fd.Layer);

            try
            {
                var ex = new Exception("Something bad has happened!");
                ex.Data.Add("input param", "notthing to see hee");
                throw ex;
            }
            catch (Exception exception)
            {
                fd = GetFlogDetail("", exception);
                Flogger.WriteError(fd);
            }

            fd = GetFlogDetail("used flogging console", null);
            Flogger.WriteUsage(fd);

            fd = GetFlogDetail("stopping app", null);
            Flogger.WriteDiagnostic(fd);

            tracker.Stop();
        }

        private static FlogDetail GetFlogDetail(string message, Exception exception)
        {
            return new FlogDetail
            {
                Product = "Flogger",
                Location = "Flogger.Console",
                Layer = "Job",
                UserName = Environment.UserName,
                Hostname = Environment.MachineName,
                Message = message,
                Exception = exception
            };
        }
    }
}
