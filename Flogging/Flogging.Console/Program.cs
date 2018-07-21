using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Flogging.Console.Models;
using Flogging.Core;
using Flogging.Data.CustomADO;
using Flogging.Data.CustomDapper;

namespace Flogging.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fd = GetFlogDetail("starting apllication", null);
            Flogger.WriteDiagnostic(fd);

            var tracker = new PerfTracker("Flogger.Console_Execution", "", fd.UserName, fd.Location, fd.Product, fd.Layer);

            //try
            //{
            //    var ex = new Exception("Something bad has happened!");
            //    ex.Data.Add("input param", "notthing to see hee");
            //    throw ex;
            //}
            //catch (Exception exception)
            //{
            //    fd = GetFlogDetail("", exception);
            //    Flogger.WriteError(fd);
            //}

            var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                try
                {
                    // Raw ADO.NET
                    //var rawAdoSp = new SqlCommand("CreateNewCustomer", db)
                    //{
                    //    CommandType = CommandType.StoredProcedure
                    //};
                    //rawAdoSp.Parameters.Add(new SqlParameter("@Name", "waytooloongforistowngood"));
                    //rawAdoSp.Parameters.Add(new SqlParameter("@TotalPerchases", 12000));
                    //rawAdoSp.Parameters.Add(new SqlParameter("@TotalReturns", 100.50M));
                    //rawAdoSp.ExecuteNonQuery();

                    var sp = new Sproc(db, "CreateNewCustomer");
                    sp.SetParam("@Name", "waytooloongforistowngood");
                    sp.SetParam("@TotalPerchases", 12000);
                    sp.SetParam("@TotalReturns", 100.50M);
                    sp.ExecNonQuery();
                }
                catch (Exception exception)
                {
                    var edf = GetFlogDetail("", exception);
                    Flogger.WriteError(edf);
                }

                try
                {
                    //Dapper
                    //db.Execute("CreateNewCustomer", new
                    //{
                    //    Name = "dappernametooloongforistowngood",
                    //    TotalPerchases = 12000,
                    //    TotalReturns = 100.50M
                    //}, commandType: CommandType.StoredProcedure);

                    db.DapperProcNonQuery("CreateNewCustomer", new
                    {
                        Name = "dappernametooloongforistowngood",
                        TotalPerchases = 12000,
                        TotalReturns = 100.50M
                    });
                }
                catch (Exception exception)
                {
                    var edf = GetFlogDetail("", exception);
                    Flogger.WriteError(edf);
                }
            }

            var ctx = new CustomerDbContext();
            try
            {
                // Entity Framework
                var name = new SqlParameter("@Name", "eftooloongforistowngood");
                var totalPerchases = new SqlParameter("@TotalPerchases", 12000);
                var totalReturns = new SqlParameter("@TotalReturns", 100.50M);
                ctx.Database.ExecuteSqlCommand("EXEC dbo.CreateNewCustomer @Name, @TotalPerchases, @TotalReturns",
                    name, totalPerchases, totalReturns);
            }
            catch (Exception exception)
            {
                var edf = GetFlogDetail("", exception);
                Flogger.WriteError(edf);
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
