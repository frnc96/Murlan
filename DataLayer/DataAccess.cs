using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class DataAccess
    {
        /// <summary>
        /// for read update delete operations
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool ExecuteQuery(string connectionString, string command)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlCon;

            sqlCommand.CommandText = command;
            int result = sqlCommand.ExecuteNonQuery();
            sqlCon.Close();
            if (result > 0)
            {
                return true;
            }
            else
                return false;
        }

        public static DataTable GetDatatable(string connectionString, string selectcommand)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter sqla = new SqlDataAdapter(selectcommand, sqlCon);
            DataSet ds = new DataSet();
            sqla.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }
    }
}
