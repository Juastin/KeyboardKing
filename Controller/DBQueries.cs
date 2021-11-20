using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class DBQueries
    {
        /// <summary>
        /// Example query.
        /// </summary>
        /// 

        public static List<List<string>> GetAllUsers()
        {
            return DBHandler.Query("SELECT id, username FROM [dbo].[User]");
        }

        public static bool AddUser(string username, string email, string password)
        {

            /*  DBHandler.Query($"INSERT INTO User (username, email, password) VALUES ({username}, {email}, {password})");*/
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[User] (username, email, password) VALUES (@username, @email, @password)", null);
                
                SqlParameter usernameParam = new SqlParameter("@username", SqlDbType.Text, 255);
                SqlParameter emailParam = new SqlParameter("@email", SqlDbType.Text, 255);
                SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.Text, 255);

                usernameParam.Value = username;
                emailParam.Value = email;
                passwordParam.Value = password;

                cmd.Parameters.Add(usernameParam);
                cmd.Parameters.Add(emailParam);
                cmd.Parameters.Add(passwordParam);

            return DBHandler.InsertQuery(cmd);
        }
    }
}
