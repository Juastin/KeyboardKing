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
            return DBHandler.SelectQuery(new SqlCommand("SELECT id, username FROM [dbo].[User]", null));
        }

        public static bool AddUser(string username, string email, string password, string salt)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[User] (username, email, password, salt) VALUES (@username, @email, @password, @salt)", null);

            SqlParameter usernameParam = new SqlParameter("@username", SqlDbType.Text, 255);
            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.Text, 255);
            SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.Text, 255);
            SqlParameter saltParam = new SqlParameter("@salt", SqlDbType.Text, 255);

            usernameParam.Value = username;
            emailParam.Value = email;
            passwordParam.Value = password;
            saltParam.Value = password;

            cmd.Parameters.Add(usernameParam);
            cmd.Parameters.Add(emailParam);
            cmd.Parameters.Add(passwordParam);
            cmd.Parameters.Add(saltParam);

            return DBHandler.Query(cmd);
        }

        public static string GetEmail(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT email FROM[dbo].[User] WHERE email = @email", null);

            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 1 ? result[0][0] : "F";
        }

        public static byte[] GetPassword(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT password FROM[dbo].[User] WHERE email = @email", null);
            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 1 ? Convert.FromBase64String(result[0][0]) : Array.Empty<byte>();
        }

        public static byte[] GetSalt(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT salt FROM[dbo].[User] WHERE email = @email", null);
            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 1 ? Convert.FromBase64String(result[0][0]) : Array.Empty<byte>();
        }

    }
}
