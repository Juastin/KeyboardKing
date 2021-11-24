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
            saltParam.Value = salt;

            cmd.Parameters.Add(usernameParam);
            cmd.Parameters.Add(emailParam);
            cmd.Parameters.Add(passwordParam);
            cmd.Parameters.Add(saltParam);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetUserInfo(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT email, password, salt FROM[dbo].[User] WHERE email = @email", null);

            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 3 ? result : new List<List<string>>();
        }

        public static List<List<string>> GetAllEpisodes()
        {
            SqlCommand cmd = new SqlCommand("SELECT [dbo].[Chapter].name, episode, [dbo].[Episode].name, [dbo].[Episode].id " +
                                    "FROM [dbo].[Episode]" +
                                   "LEFT JOIN [dbo].[Chapter]" +
                                   "ON [dbo].[Episode].chapterid = [dbo].[Chapter].id", null);
            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllEpisodeStepsFromEpisode(string id)
        {
            SqlCommand cmd = new SqlCommand("SELECT word " +
                "FROM [dbo].[EpisodeStep] " +
                "WHERE episodeid = @id", null);

            SqlParameter episodeIdParam = new SqlParameter("@id", SqlDbType.Int, 255);
            episodeIdParam.Value = id;
            cmd.Parameters.Add(episodeIdParam);

            return DBHandler.SelectQuery(cmd);
        }

    }
}
