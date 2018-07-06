using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Murlan.Models
{
    public class Business
    {
        public static Boolean Authenticate(string email, string pass)
        {
            SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStringDb"].ConnectionString);
            myConnection.Open();
            string query = "SELECT * FROM [SoftwareArchitecture].[dbo].[UsersTbl] WHERE [user_email] = '"+email+"' AND [user_password] = '"+pass+"'";

            SqlCommand selectCommand = new SqlCommand(query, myConnection);
            SqlDataReader result = selectCommand.ExecuteReader();
            //myConnection.Close();

            if (result.Read())
            {
                StaticClass.UserEmail = result.GetString(1);
                //StaticClass.UserId = int.Parse(result.GetValue(0));
                myConnection.Close();
                return true;
            }
            else
            {
                myConnection.Close();
                return false;
            }
        }

        public static Boolean ServerAuthentication(string email, string pass)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnStringDb"].ConnectionString;
            string query = "SELECT * FROM [SoftwareArchitecture].[dbo].[UsersTbl] WHERE [user_email] = '" + email + "' AND [user_password] = '" + pass + "'";
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (email.Equals("") || pass.Equals("") || !match.Success)
            {
                return false;
            }
            else
            {
                DataTable dt = DataLayer.DataAccess.GetDatatable(connString, query);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;

            }
        }

        public static Boolean DbUpdate(string email, string name, string surname, string pass, string confPass)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnStringDb"].ConnectionString;
            string command = "insert into [SoftwareArchitecture].[dbo].[UsersTbl] (user_email, user_name, user_surname, user_password)" +
                "values ('"+email+ "','"+name+ "','"+surname+ "','"+pass+"')";
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success && pass.Equals(confPass))
                return (DataLayer.DataAccess.ExecuteQuery(connString, command));
            else
                return false;
        }
    }
}