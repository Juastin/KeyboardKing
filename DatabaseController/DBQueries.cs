using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
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
            return DBHandler.SelectQuery(new MySqlCommand("SELECT id, username FROM User"));
        }

        public static bool AddUser(string username, string email, string password, string salt)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO User (username, email, password, salt) VALUES (@username, @email, @password, @salt)");

            MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.VarChar, 255);
            MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar, 255);
            MySqlParameter passwordParam = new MySqlParameter("@password", MySqlDbType.VarChar, 255);
            MySqlParameter saltParam = new MySqlParameter("@salt", MySqlDbType.VarChar, 255);

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
            MySqlCommand cmd = new MySqlCommand("UPDATE UserSettings set skilllevel = @skill WHERE userid = @id");

            MySqlParameter skilllevelParam = new MySqlParameter("@skill", MySqlDbType.VarChar, 255);
            MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32, 0);

            skilllevelParam.Value = skilllevel;
            idParam.Value = data.Get<int>(0);

            cmd.Parameters.Add(skilllevelParam);
            cmd.Parameters.Add(idParam);

            return DBHandler.Query(cmd);
        }

        public static bool SaveResult(EpisodeResult es, int episodeId, int userId)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO EpisodeResult (episodeid, userid, score, mistakes, lettersperminute, time) " +
                                            "VALUES (@episodeid, @userid, @score, @mistakes, @lpm, @time)");
            MySqlParameter episodeidParam = new MySqlParameter("@episodeid", MySqlDbType.Int32, 0);
            MySqlParameter useridParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            MySqlParameter scoreParam = new MySqlParameter("@score", MySqlDbType.Int32, 0);
            MySqlParameter mistakesParam = new MySqlParameter("@mistakes", MySqlDbType.Int32, 0);
            MySqlParameter lpmParam = new MySqlParameter("@lpm", MySqlDbType.Int32, 0);
            MySqlParameter timeParam = new MySqlParameter("@time", MySqlDbType.Int32, 0);

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
 
            MySqlCommand cmd = new MySqlCommand("UPDATE MatchProgress set score = @score, mistakes = @mistakes, lettersperminute = @lpm, time = @time " +
                                "WHERE userId = @userid AND matchId = @matchid");

            MySqlParameter matchidParam = new MySqlParameter("@matchid", MySqlDbType.Int32, 0);
            MySqlParameter useridParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            MySqlParameter scoreParam = new MySqlParameter("@score", MySqlDbType.Int32, 0);
            MySqlParameter mistakesParam = new MySqlParameter("@mistakes", MySqlDbType.Int32, 0);
            MySqlParameter lpmParam = new MySqlParameter("@lpm", MySqlDbType.Int32, 0);
            MySqlParameter timeParam = new MySqlParameter("@time", MySqlDbType.Int32, 0);

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
            MySqlCommand cmd = new MySqlCommand("SELECT id, username, email, password, salt, skilllevel " +
                                            "FROM User " +
                                            "LEFT JOIN UserSettings " +
                                            "ON User.id = UserSettings.userid " +
                                            "WHERE email = @email");

            MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result.Count == 1 && result[0].Count == 6 ? result : new List<List<string>>();
        }

        public static List<List<string>> GetAllEpisodes(UList user)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT c.name, episode, e.name, e.id, " +
                "CASE WHEN er.userid IS NULL THEN 'False' ELSE 'True' END AS completed, " +
                "MAX(er.score) AS highscore " +
                "FROM Episode e " +
                "LEFT JOIN Chapter c " +
                "ON e.chapterid = c.id " +
                "LEFT JOIN EpisodeResult er " +
                "ON er.episodeid = e.id " +
                "AND er.userid = @userid " +
                "GROUP BY c.name, episode, e.name, e.id, er.userid");

            MySqlParameter UserIdParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            UserIdParam.Value = user.Get<int>(0);

            cmd.Parameters.Add(UserIdParam);

            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllEpisodeStepsFromEpisode(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT word " +
                "FROM EpisodeStep " +
                "WHERE episodeid = @id " +
                "ORDER BY RAND()");

            MySqlParameter episodeIdParam = new MySqlParameter("@id", MySqlDbType.Int32, 255);
            episodeIdParam.Value = id;
            cmd.Parameters.Add(episodeIdParam);

            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllActiveMatches()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(mp.id) playercount, u.username as host, m.id, e.name " +
                "FROM MatchLobby m " +
                "LEFT JOIN MatchProgress mp ON m.id = mp.matchid " +
                "LEFT JOIN User u ON m.creatorid = u.id " +
                "LEFT JOIN Episode e ON m.episodeid = e.id " +
                "WHERE m.state = 0 " + 
                "GROUP BY m.id, u.username, m.id, e.name");
            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllUsersInMatch()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT userid " +
                                            "FROM MatchLobby m " +
                                            "RIGHT JOIN MatchProgress mp ON m.id = mp.matchid " +
                                            "WHERE m.state != 2 ");
            return DBHandler.SelectQuery(cmd);
        }

        public static bool SetPlayState(int matchid, int state)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE MatchLobby set state = @state WHERE id = @matchid ");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 0);
            MySqlParameter State = new MySqlParameter("@state", MySqlDbType.Int32, 0);

            matchId.Value = matchid;
            State.Value = state;

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(State);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateMatchProgress(int progress, int user_id, int match_id)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE MatchProgress set progress = @progress WHERE userid = @userid AND matchid = @matchid ");

            MySqlParameter q_progress = new MySqlParameter("@progress", MySqlDbType.Int32, 100);
            MySqlParameter q_user_id = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            MySqlParameter q_match_id = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);

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
            MySqlCommand cmd = new MySqlCommand("SELECT TOP 4 username, progress FROM MatchProgress mp LEFT JOIN User u ON mp.userid=u.id WHERE userid != @userid AND matchid = @matchid ORDER BY progress DESC");

            MySqlParameter q_user_id = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            MySqlParameter q_match_id = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);

            q_user_id.Value = user_id;
            q_match_id.Value = match_id;

            cmd.Parameters.Add(q_user_id);
            cmd.Parameters.Add(q_match_id);

            return DBHandler.SelectQuery(cmd);
        }

        public static int AddMatch(int episodeid, UList user)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO MatchLobby (episodeid, creatorid) VALUES (@episodeid, @creatorid)");

            MySqlParameter episodeId = new MySqlParameter("@episodeid", MySqlDbType.Int32, 255);
            MySqlParameter creatorId = new MySqlParameter("@creatorid", MySqlDbType.Int32, 0);

            episodeId.Value = episodeid;
            creatorId.Value = user.Get<int>(0);

            cmd.Parameters.Add(episodeId);
            cmd.Parameters.Add(creatorId);

            return DBHandler.InsertAndGet(cmd);
        }

        public static bool AddMatchProgress(int matchid, UList user)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO MatchProgress (matchid, userid) " +
                                            "VALUES (@matchid, @userid)");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            matchId.Value = matchid;
            userId.Value = user.Get<int>(0);

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(userId);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetMatchProgress(int matchid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT mp.matchid, u.username, e.name, mp.progress, mp.score, mp.mistakes, mp.lettersperminute, mp.time, m.creatorid, m.episodeid, m.state, mp.userid " +
             "FROM MatchProgress mp " +
             "LEFT JOIN MatchLobby m ON mp.matchid = m.id " +
             "LEFT JOIN Episode e ON m.episodeid = e.id " +
             "LEFT JOIN User u ON mp.userid = u.id " +
             "WHERE mp.matchid = @matchid");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            matchId.Value = matchid;
            cmd.Parameters.Add(matchId);
            return DBHandler.SelectQuery(cmd);
        }

        public static bool RemoveUserInMatch(int matchid, UList user)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM MatchProgress WHERE matchid = @matchid AND userid = @userid;");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            matchId.Value = matchid;
            userId.Value = user.Get<int>(0);

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(userId);

            return DBHandler.Query(cmd);
        }

        public static bool DeleteMatch(int matchid)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM MatchLobby WHERE id = @matchid;");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            matchId.Value = matchid;
            cmd.Parameters.Add(matchId);
            return DBHandler.Query(cmd);
        }

        public static bool UpdateNewCreatorInMatch(int matchid, int newcreatorid)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE MatchLobby SET creatorid = @newcreatorid WHERE id = @matchid");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            MySqlParameter newcreatorId = new MySqlParameter("@newcreatorid", MySqlDbType.Int32, 0);

            matchId.Value = matchid;
            newcreatorId.Value = newcreatorid;

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(newcreatorId);

            return DBHandler.Query(cmd);
        }

        public static List<List<string>> GetScoresOrderByHighest(int matchid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT TOP 3 u.username, mp.score " +
            "FROM MatchProgress mp " +
            "LEFT JOIN User u ON mp.userid = u.id " +
            "WHERE mp.matchid = @matchid " +
            "ORDER BY mp.score DESC");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);

            matchId.Value = matchid;

            cmd.Parameters.Add(matchId);

            return DBHandler.SelectQuery(cmd);
        }

        public static List<List<string>> GetAllProgress(int match_id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT progress FROM MatchProgress WHERE matchid = @matchid");

            MySqlParameter q_match_id = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);

            q_match_id.Value = match_id;

            cmd.Parameters.Add(q_match_id);

            return DBHandler.SelectQuery(cmd);
        }

    }
}
