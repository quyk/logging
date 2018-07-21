using System;
using System.Data;
using Dapper;

namespace Flogging.Data.CustomDapper
{
    public static class DapperExtensions
    {
        public static int DapperProcNonQuery(this IDbConnection connection, string procName, 
            object paramList = null, IDbTransaction transaction = null, int? timeoutSeconds = null)
        {
            try
            {
                return connection.Execute(procName, paramList, transaction, timeoutSeconds,
                    CommandType.StoredProcedure);
            }
            catch (Exception exception)
            {
                var newException = new Exception("Dapper proc execute failed", exception);
                AddDetailsToException(newException, procName, paramList);
                throw newException;
            }
        }

        private static void AddDetailsToException(Exception newException, string procName, object paramList)
        {
            newException.Data.Add("Procedure", procName);
            if (paramList is DynamicParameters dynamicParameters)
            {
                foreach (var dynamicParameter in dynamicParameters.ParameterNames)
                {
                    newException.Data.Add(dynamicParameter, dynamicParameters.Get<object>(dynamicParameter).ToString());
                }
            }
            else
            {
                var props = paramList.GetType().GetProperties();
                foreach (var prop in props)
                {
                    newException.Data.Add(prop.Name, prop.GetValue(paramList).ToString());
                }
            }
        }
    }
}