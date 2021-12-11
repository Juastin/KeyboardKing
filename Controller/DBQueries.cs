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
            return DBHandler.SelectQuery(new SqlCommand("SELECT id, username FROM [dbo].[User]"));
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

        public static bool AddSkill(string skilllevel, UList data)
        {
            SqlCommand cmd = new SqlCommand("UPDATE [dbo].[UserSettings] set skilllevel = @skill WHERE userid = @id");

            SqlParameter skilllevelParam = new SqlParameter("@skill", SqlDbType.VarChar, 255);
            SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 0);

            skilllevelParam.Value = skilllevel;
            idParam.Value = data.Get<int>(0);

            cmd.Parameters.Add(skilllevelParam);
            cmd.Parameters.Add(idParam);

            return DBHandler.Query(cmd);
        }

        public static bool SaveResult(EpisodeResult es, int episodeId, int userId)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[EpisodeResult] (episodeid, userid, score, mistakes, lettersperminute, time) " +
                                            "VALUES (@episodeid, @userid, @score, @mistakes, @lpm, @time)");
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

        public static bool SaveMatchResult(EpisodeResult es, int matchId, int userId)
        {
 
            SqlCommand cmd = new SqlCommand("UPDATE [dbo].[MatchProgress] set score = @score, mistakes = @mistakes, lettersperminute = @lpm, time = @time " +
                                "WHERE userId = @userid AND matchId = @matchid");

            SqlParameter matchidParam = new SqlParameter("@matchid", SqlDbType.Int, 0);
            SqlParameter useridParam = new SqlParameter("@userid", SqlDbType.Int, 0);
            SqlParameter scoreParam = new SqlParameter("@score", SqlDbType.Int, 0);
            SqlParameter mistakesParam = new SqlParameter("@mistakes", SqlDbType.Int, 0);
            SqlParameter lpmParam = new SqlParameter("@lpm", SqlDbType.Int, 0);
            SqlParameter timeParam = new SqlParameter("@time", SqlDbType.BigInt, 0);

            matchidParam.Value = matchId;
            useridParam.Value = userId;
            mistakesParam.Value = es.Mistakes;
            scoreParam.Value = es.Score;
            lpmParam.Value = es.LettersPerMinute;
            timeParam.Value = es.Time.Ticks;

            cmd.Parameters.Add(matchidParam);
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
                                            "WHERE email = @email");

            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 6 ? result : new List<List<string>>();
        }

        public static List<List<string>> GetAllEpisodes(UList user)
        {
            SqlCommand cmd = new SqlCommand("SELECT c.name, episode, e.name, e.id, " +
                "CASE WHEN er.userid IS NULL THEN 'False' ELSE 'True' END AS completed, " +
                "MAX(er.score) AS highscore " +
                "FROM [dbo].[Episode] e " +
                "LEFT JOIN [dbo].[Chapter] c " +
                "ON e.chapterid = c.id " +
                "LEFT JOIN [dbo].[EpisodeResult] er " +
                "ON er.episodeid = e.id " +
                "AND er.userid = @userid " +
                "GROUP BY c.name, episode, e.name, e.id, er.userid");

            SqlParameter UserIdParam = new SqlParameter("@userid", SqlDbType.Int, 0);
            UserIdParam.Value = user.Get<int>(0);

            cmd.Parameters.Add(UserIdParam);

            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllEpisodeStepsFromEpisode(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT word " +
                "FROM [dbo].[EpisodeStep] " +
                "WHERE episodeid = @id " +
                "ORDER BY NEWID()");

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
                "WHERE m.state = 0 " + 
                "GROUP BY m.id, u.username, m.id, e.name");
            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllUsersInMatch()
        {
            SqlCommand cmd = new SqlCommand("SELECT userid " +
                                            "FROM [dbo].[Match] m " +
                                            "RIGHT JOIN [dbo].[MatchProgress] mp ON m.id = mp.matchid " +
                                            "WHERE m.state != 2 ");
            return DBHandler.SelectQuery(cmd);
        }

        public static bool SetPlayState(int matchid, int state)
        {
            SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Match] set state = @state WHERE id = @matchid ");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 0);
            SqlParameter State = new SqlParameter("@state", SqlDbType.Int, 0);

            matchId.Value = matchid;
            State.Value = state;

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(State);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateMatchProgress(int progress, int user_id, int match_id)
        {
            SqlCommand cmd = new SqlCommand("UPDATE [dbo].[MatchProgress] set progress = @progress WHERE userid = @userid AND matchid = @matchid ");

            SqlParameter q_progress = new SqlParameter("@progress", SqlDbType.Int, 100);
            SqlParameter q_user_id = new SqlParameter("@userid", SqlDbType.Int, 255);
            SqlParameter q_match_id = new SqlParameter("@matchid", SqlDbType.Int, 255);

            q_progress.Value = progress;
            q_user_id.Value = user_id;
            q_match_id.Value = match_id;

            cmd.Parameters.Add(q_progress);
            cmd.Parameters.Add(q_user_id);
            cmd.Parameters.Add(q_match_id);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetOpponentProgress(int user_id, int match_id)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 4 username, progress FROM [dbo].[MatchProgress] mp LEFT JOIN [dbo].[User] u ON mp.userid=u.id WHERE userid != @userid AND matchid = @matchid ORDER BY progress DESC");

            SqlParameter q_user_id = new SqlParameter("@userid", SqlDbType.Int, 255);
            SqlParameter q_match_id = new SqlParameter("@matchid", SqlDbType.Int, 255);

            q_user_id.Value = user_id;
            q_match_id.Value = match_id;

            cmd.Parameters.Add(q_user_id);
            cmd.Parameters.Add(q_match_id);

            return DBHandler.SelectQuery(cmd);
        }

        public static int AddMatch(int episodeid, UList user)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Match] (episodeid, creatorid) output INSERTED.id VALUES(@episodeid, @creatorid)");

            SqlParameter episodeId = new SqlParameter("@episodeid", SqlDbType.Int, 255);
            SqlParameter creatorId = new SqlParameter("@creatorid", SqlDbType.Int, 0);

            episodeId.Value = episodeid;
            creatorId.Value = user.Get<int>(0);

            cmd.Parameters.Add(episodeId);
            cmd.Parameters.Add(creatorId);

            return DBHandler.QueryScalar<int>(cmd);
        }

        public static bool AddMatchProgress(int matchid, UList user)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[MatchProgress] (matchid, userid) " +
                                            "VALUES (@matchid, @userid)");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 255);
            SqlParameter userId = new SqlParameter("@userid", SqlDbType.Int, 0);

            matchId.Value = matchid;
            userId.Value = user.Get<int>(0);

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(userId);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetMatchProgress(int matchid)
        {
            SqlCommand cmd = new SqlCommand("SELECT mp.matchid, u.username, e.name, mp.progress, mp.score, mp.mistakes, mp.lettersperminute, mp.time, m.creatorid, m.episodeid, m.state, mp.userid " +
             "FROM [dbo].[MatchProgress] mp " +
             "LEFT JOIN [dbo].[Match] m ON mp.matchid = m.id " +
             "LEFT JOIN [dbo].[Episode] e ON m.episodeid = e.id " +
             "LEFT JOIN [dbo].[User] u ON mp.userid = u.id " +
             "WHERE mp.matchid = @matchid");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 255);
            matchId.Value = matchid;
            cmd.Parameters.Add(matchId);
            return DBHandler.SelectQuery(cmd);
        }

        public static bool RemoveUserInMatch(int matchid, UList user)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[MatchProgress] WHERE matchid = @matchid AND userid = @userid;");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 255);
            SqlParameter userId = new SqlParameter("@userid", SqlDbType.Int, 0);

            matchId.Value = matchid;
            userId.Value = user.Get<int>(0);

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(userId);

            return DBHandler.Query(cmd);
        }

        public static bool DeleteMatch(int matchid)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[Match] WHERE id = @matchid;");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 255);
            matchId.Value = matchid;
            cmd.Parameters.Add(matchId);
            return DBHandler.Query(cmd);
        }

        public static bool UpdateNewCreatorInMatch(int matchid, int newcreatorid)
        {
            SqlCommand cmd = new SqlCommand("UPDATE FROM [dbo].[Match] SET creatorid = @newcreatorid WHERE matchid = @matchid");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 255);
            SqlParameter newcreatorId = new SqlParameter("@newcreatorid", SqlDbType.Int, 0);

            matchId.Value = matchid;
            newcreatorId.Value = newcreatorid;

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(newcreatorId);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetScoresOrderByHighest(int matchid)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 3 u.username, mp.score " +
            "FROM [dbo].[MatchProgress] mp " +
            "LEFT JOIN [dbo].[User] u ON mp.userid = u.id " +
            "WHERE mp.matchid = @matchid " +
            "ORDER BY mp.score DESC");

            SqlParameter matchId = new SqlParameter("@matchid", SqlDbType.Int, 255);

            matchId.Value = matchid;

            cmd.Parameters.Add(matchId);

            return DBHandler.SelectQuery(cmd);
        }
    }
}
