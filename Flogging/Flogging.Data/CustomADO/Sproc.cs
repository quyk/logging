using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Flogging.Data.CustomADO
{
    public class Sproc
    {
        private SqlCommand Command { get; set; }

        public Sproc(SqlConnection connection, string procName, int timeoutSeconds = 30)
        {
            Command = new SqlCommand(procName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (timeoutSeconds != 30)
            {
                Command.CommandTimeout = timeoutSeconds;
            }
        }

        public void SetParam(string paramName, object value)
        {
            Command.Parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
        }

        public int ExecNonQuery()
        {
            try
            {
                return Command.ExecuteNonQuery(); //return nymber of rows affected
            }
            catch (Exception exception)
            {
                throw CreateProcedureException(exception);
            }
        }

        private Exception CreateProcedureException(Exception exception)
        {
            var newException = new Exception("Stored Procedure call fail!", exception);
            newException.Data.Add("Procedure:", Command.CommandText);
            newException.Data.Add("ProcInputs:", GetInputString());
            return newException;
        }

        private string GetInputString()
        {
            var inString = new StringBuilder();
            foreach (SqlParameter commandParameter in Command.Parameters)
            {
                inString.Append($"{commandParameter.ParameterName} = {commandParameter.Value} |");
            }

            return inString.ToString();
        }
    }
}
