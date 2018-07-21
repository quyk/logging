using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Flogging.Data.CustomEntityFramework
{
    public class FloggerEfInterceptor : IDbCommandInterceptor
    {
        private Exception WrapEntityFrameworkException(DbCommand command, Exception exception)
        {
            var newException = new Exception("EntityFramework command failed!", exception);
            AddParamsToException(command.Parameters, newException);
            return newException;
        }

        private void AddParamsToException(DbParameterCollection commandParameters, Exception newException)
        {
            foreach (DbParameter commandParameter in commandParameters)
            {
                newException.Data.Add(commandParameter.ParameterName, commandParameter.Value.ToString());
            }
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
            }
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
            }
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
            }
        }
    }
}