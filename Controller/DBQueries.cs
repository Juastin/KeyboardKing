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
        private static SqlConnection _connection { get; set; } = new SqlConnection(TripleDES.Decrypt(ConfigurationManager.AppSettings["connectionString"], "332cc6da-d757-4e80-a726-0bf6b615df09"));

        public static List<List<string>> GetAllUsers()
        {
            return DBHandler.Query("SELECT id, username FROM [dbo].[User]");
        }

        public static bool AddUser(string username, string email, string password)
        {

            /*  DBHandler.Query($"INSERT INTO User (username, email, password) VALUES ({username}, {email}, {password})");*/

            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[User] (username, email, password) VALUES (@username, @email, @password)", _connection);
                

                SqlParameter usernameParam = new SqlParameter("@username", SqlDbType.VarChar, 255);
                SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
                SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar, 255);

                usernameParam.Value = username;
                emailParam.Value = email;
                passwordParam.Value = password;

                cmd.Parameters.Add(usernameParam);
                cmd.Parameters.Add(emailParam);
                cmd.Parameters.Add(passwordParam);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {

                if (_connection != null) { _connection.Close(); }
            }

        }
    }
}
