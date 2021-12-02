using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[User] (username, email, password, salt) VALUES (@username, @email, @password, @salt)");


            SqlParameter usernameParam = new SqlParameter("@username", SqlDbType.VarChar, 255);
            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar, 255);
            SqlParameter saltParam = new SqlParameter("@salt", SqlDbType.VarChar, 255);

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

   

        public static bool AddSkill(string skilllevel, string[] Data)
        {
            SqlCommand cmd = new SqlCommand("UPDATE [dbo].[UserSettings] set skilllevel = @skill WHERE userid = @id");

            SqlParameter skilllevelParam = new SqlParameter("@skill", SqlDbType.VarChar, 255);
            SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 0);


            skilllevelParam.Value = skilllevel;
            idParam.Value = Data[0];

            cmd.Parameters.Add(skilllevelParam);
            cmd.Parameters.Add(idParam);

            return DBHandler.Query(cmd);
        }

        public static bool SaveResult(EpisodeResult es, int episodeId, int userId)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[EpisodeResult] (episodeid, userid, score, mistakes, lettersperminute, time) " +
                                            "VALUES (@episodeid, @userid, @score, @mistakes, @lpm, @time)", null);
            SqlParameter episodeidParam = new SqlParameter("@episodeid", SqlDbType.Int, 0);
            SqlParameter useridParam = new SqlParameter("@userid", SqlDbType.Int, 0);
            SqlParameter scoreParam = new SqlParameter("@score", SqlDbType.Int, 0);
            SqlParameter mistakesParam = new SqlParameter("@mistakes", SqlDbType.Int, 0);
            SqlParameter lpmParam = new SqlParameter("@lpm", SqlDbType.Int, 0);
            SqlParameter timeParam = new SqlParameter("@time", SqlDbType.BigInt, 0);

            episodeidParam.Value = episodeId;
            useridParam.Value = userId;
            mistakesParam.Value = es.Mistakes;
            scoreParam.Value = es.Score;
            lpmParam.Value = es.LettersPerMinute;
            timeParam.Value = es.Time.Ticks;

            cmd.Parameters.Add(episodeidParam);
            cmd.Parameters.Add(useridParam);
            cmd.Parameters.Add(mistakesParam);
            cmd.Parameters.Add(scoreParam);
            cmd.Parameters.Add(lpmParam);
            cmd.Parameters.Add(timeParam);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetUserInfo(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT id, username, email, password, salt, skilllevel " +
                                            "FROM[dbo].[User] " +
                                            "LEFT JOIN [dbo].[UserSettings] " +
                                            "ON [dbo].[User].id = [dbo].[UserSettings].userid " +
                                            "WHERE email = @email", null);

            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 6 ? result : new List<List<string>>();
        }

        public static List<List<string>> GetAllEpisodes(string[] User)
        {
            SqlCommand cmd = new SqlCommand("SELECT [dbo].[Chapter].name, episode, [dbo].[Episode].name, [dbo].[Episode].id, " +
                "CASE WHEN [dbo].[EpisodeResult].userid IS NULL THEN 'False' ELSE 'True' END AS completed, " +
                "MAX([dbo].[EpisodeResult].score) AS highscore " +
                "FROM [dbo].[Episode] " +
                "LEFT JOIN [dbo].[Chapter] " +
                "ON [dbo].[Episode].chapterid = [dbo].[Chapter].id " +
                "LEFT JOIN [dbo].[EpisodeResult] " +
                "ON [dbo].[EpisodeResult].episodeid = [dbo].[Episode].id " +
                "AND [dbo].[EpisodeResult].userid = @userid " +
                "GROUP BY [dbo].[Chapter].name, episode, [dbo].[Episode].name, [dbo].[Episode].id, [dbo].[EpisodeResult].userid");

            SqlParameter UserIdParam = new SqlParameter("@userid", SqlDbType.Int, 0);
            UserIdParam.Value = User[0];

            cmd.Parameters.Add(UserIdParam);

            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllEpisodeStepsFromEpisode(string id)
        {
            SqlCommand cmd = new SqlCommand("SELECT word " +
                "FROM [dbo].[EpisodeStep] " +
                "WHERE episodeid = @id " +
                "ORDER BY NEWID()", null);

            SqlParameter episodeIdParam = new SqlParameter("@id", SqlDbType.Int, 255);
            episodeIdParam.Value = id;
            cmd.Parameters.Add(episodeIdParam);

            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllActiveMatches()
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(mp.id) playercount, u.username as host, m.id, e.name " +
                "FROM [dbo].[Match] m " +
                "LEFT JOIN [dbo].[MatchProgress] mp ON m.id = mp.matchid " +
                "LEFT JOIN [dbo].[User] u ON m.creatorid = u.id " +
                "LEFT JOIN [dbo].[Episode] e ON m.episodeid = e.id " +
                "GROUP BY m.id, u.username, m.id, e.name", null);
            return DBHandler.SelectQuery(cmd);
        }

        public static bool AddMatch(int episodeid, string[] User)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Match] (episodeid, creatorid) VALUES (@episodeid, @creatorid)", null);

            SqlParameter episodeId = new SqlParameter("@episodeid", SqlDbType.Int, 255);
            SqlParameter creatorId = new SqlParameter("@creatorid", SqlDbType.Int, 0);

            episodeId.Value = episodeid;
            creatorId.Value = User[0];

            cmd.Parameters.Add(episodeId);
            cmd.Parameters.Add(creatorId);

            return DBHandler.Query(cmd);
        }
    }
}
