using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Model;

namespace DatabaseController
{
    public static class DBQueries
    {
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

        public static bool AddSkill(string skilllevel, User user)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE UserSettings set skilllevel = @skill WHERE userid = @id");

            MySqlParameter skilllevelParam = new MySqlParameter("@skill", MySqlDbType.VarChar, 255);
            MySqlParameter idParam = new MySqlParameter("@id", MySqlDbType.Int32, 0);

            skilllevelParam.Value = skilllevel;
            idParam.Value = user.Id;

            cmd.Parameters.Add(skilllevelParam);
            cmd.Parameters.Add(idParam);

            return DBHandler.Query(cmd);
        }

        public static bool SaveResult(EpisodeResult es, int episodeId, int userId)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO EpisodeResult (episodeid, userid, score, mistakes, lettersperminute, time, passed) " +
                                            "VALUES (@episodeid, @userid, @score, @mistakes, @lpm, @time, @passed)");
            MySqlParameter episodeidParam = new MySqlParameter("@episodeid", MySqlDbType.Int32, 0);
            MySqlParameter useridParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            MySqlParameter scoreParam = new MySqlParameter("@score", MySqlDbType.Int32, 0);
            MySqlParameter mistakesParam = new MySqlParameter("@mistakes", MySqlDbType.Int32, 0);
            MySqlParameter lpmParam = new MySqlParameter("@lpm", MySqlDbType.Int32, 0);
            MySqlParameter timeParam = new MySqlParameter("@time", MySqlDbType.Int64, 0);
            MySqlParameter passedParam = new MySqlParameter("@passed", MySqlDbType.Bit, 0);

            episodeidParam.Value = episodeId;
            useridParam.Value = userId;
            mistakesParam.Value = es.Mistakes;
            scoreParam.Value = es.Score;
            lpmParam.Value = es.LettersPerMinute;
            timeParam.Value = es.Time.Ticks;
            passedParam.Value = es.Passed;

            cmd.Parameters.Add(episodeidParam);
            cmd.Parameters.Add(useridParam);
            cmd.Parameters.Add(mistakesParam);
            cmd.Parameters.Add(scoreParam);
            cmd.Parameters.Add(lpmParam);
            cmd.Parameters.Add(timeParam); 
            cmd.Parameters.Add(passedParam);

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
            MySqlParameter timeParam = new MySqlParameter("@time", MySqlDbType.Int64, 0);

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

        public static User GetUserInfo(string email)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT id, username, email, coins, password, salt, skilllevel, audio, dyslectic, theme " +
                                            "FROM User " +
                                            "LEFT JOIN UserSettings " +
                                            "ON User.id = UserSettings.userid " +
                                            "WHERE email = @email");

            MySqlParameter emailParam = new MySqlParameter("@email", MySqlDbType.VarChar, 255);
            emailParam.Value = email;
            cmd.Parameters.Add(emailParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return User.ParseUser(result);
        }

        public static List<Chapter> GetAllChapters(User user)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT c.id, c.name, episode, e.name, e.id, " +
                "CASE WHEN er.passed IS NULL THEN 'False' ELSE 'True' END AS completed, " +
                "MAX(er.score) AS highscore " +
                "FROM Episode e " +
                "LEFT JOIN Chapter c " +
                "ON e.chapterid = c.id " +
                "LEFT JOIN EpisodeResult er " +
                "ON er.episodeid = e.id " +
                "AND er.userid = @userid " +
                "AND er.passed = 1 " +
                "GROUP BY e.id");

            MySqlParameter UserIdParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            UserIdParam.Value = user.Id;
            cmd.Parameters.Add(UserIdParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return Chapter.ParseAllChapters(result);
        }

        public static List<Item> GetAllItems(User user)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT i.id, i.name, i.price, i.type, " +
                "CASE WHEN ui.userid IS NULL THEN 'False' ELSE 'True' END AS completed " +
                "FROM Item i " +
                "LEFT JOIN Useritem ui ON i.id = ui.itemid " +
                "AND ui.userid = @userid");

            MySqlParameter UserIdParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            UserIdParam.Value = user.Id;

            cmd.Parameters.Add(UserIdParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return Item.ParseItems(result);
        }

        public static bool AddItem(User user, Item item)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Useritem VALUES(@userId, @itemId)");

            MySqlParameter userId = new MySqlParameter("@userId", MySqlDbType.Int32, 0);
            MySqlParameter itemID = new MySqlParameter("@itemId", MySqlDbType.Int32, 0);

            userId.Value = user.Id;
            itemID.Value = item.Id;

            cmd.Parameters.Add(userId);
            cmd.Parameters.Add(itemID);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateCoins(User user, Item item)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE User SET coins = coins - @itemPrice " +
                                                "WHERE id = @userId");

            MySqlParameter userId = new MySqlParameter("@userId", MySqlDbType.Int32, 0);
            MySqlParameter itemPrice = new MySqlParameter("@itemPrice", MySqlDbType.Int32, 0);

            userId.Value = user.Id;
            itemPrice.Value = item.Price;

            cmd.Parameters.Add(userId);
            cmd.Parameters.Add(itemPrice);

            return DBHandler.Query(cmd);
        }

        public static int CheckIfItemExists(Item item)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(id) " +
                                                "FROM Item i " +
                                                "WHERE i.id = @itemId");

            MySqlParameter itemId = new MySqlParameter("@itemId", MySqlDbType.Int32, 255);
            itemId.Value = item.Id;
            cmd.Parameters.Add(itemId);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return int.Parse(result[0][0]);
        }

        public static int CheckIfItemAlreadyBought(User user, Item item)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(itemid) FROM Useritem " +
                                                "WHERE userid = @userid " +
                                                "AND itemid = @itemid ");

            MySqlParameter userId = new MySqlParameter("@userId", MySqlDbType.Int32, 0);
            MySqlParameter itemId = new MySqlParameter("@itemId", MySqlDbType.Int32, 0);
            userId.Value = user.Id;
            itemId.Value = item.Id;
            cmd.Parameters.Add(userId);
            cmd.Parameters.Add(itemId);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return int.Parse(result[0][0]);
        }

        public static int GetHighscoreEpisode(User user, int episodeId)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT MAX(score) FROM EpisodeResult WHERE episodeid = @episodeid AND userid = @userid");


            MySqlParameter EpisodeIdParam = new MySqlParameter("@episodeid", MySqlDbType.Int32, 0);
            MySqlParameter UserIdParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            EpisodeIdParam.Value = episodeId;
            UserIdParam.Value = user.Id;

            cmd.Parameters.Add(EpisodeIdParam);
            cmd.Parameters.Add(UserIdParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
   
            return !string.IsNullOrEmpty(result[0][0]) ? int.Parse(result[0][0]) : 0;
        }

        public static void UpdateCoins(int coins, User user)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE User SET coins = coins + @coins WHERE id = @userid");

            MySqlParameter CoinsIdParam = new MySqlParameter("@coins", MySqlDbType.Int32, 0);
            MySqlParameter UserIdParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            CoinsIdParam.Value = coins;
            UserIdParam.Value = user.Id;

            cmd.Parameters.Add(CoinsIdParam);
            cmd.Parameters.Add(UserIdParam);

            DBHandler.Query(cmd);
        }

        public static int GetCoinsOfUser(User user)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT coins From User WHERE id = @userid");

            MySqlParameter UserIdParam = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            UserIdParam.Value = user.Id;

            cmd.Parameters.Add(UserIdParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);

            return int.Parse(result[0][0]);
        }

        public static List<EpisodeStep> GetAllEpisodeStepsFromEpisode(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT word " +
                "FROM EpisodeStep " +
                "WHERE episodeid = @id " +
                "ORDER BY RAND()");

            MySqlParameter episodeIdParam = new MySqlParameter("@id", MySqlDbType.Int32, 255);
            episodeIdParam.Value = id;
            cmd.Parameters.Add(episodeIdParam);
            
            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return EpisodeStep.ParseEpisodeSteps(result);
        }

        public static int Getpassthreshold(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT passthreshold " +
               "FROM Chapter c " +
               "LEFT JOIN Episode e ON c.id = e.chapterid " +
               "WHERE e.id = @id");

            MySqlParameter episodeIdParam = new MySqlParameter("@id", MySqlDbType.Int32, 255);
            episodeIdParam.Value = id;
            cmd.Parameters.Add(episodeIdParam);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return int.Parse(result[0][0]);
        }

        public static List<Match> GetAllActiveMatches()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(mp.id) playercount, u.id, u.username as host, m.id, m.state, e.id, e.name " +
                "FROM MatchLobby m " +
                "LEFT JOIN MatchProgress mp ON m.id = mp.matchid " +
                "LEFT JOIN User u ON m.creatorid = u.id " +
                "LEFT JOIN Episode e ON m.episodeid = e.id " +
                "WHERE m.state = 0 " + 
                "GROUP BY m.id, m.state, u.id, u.username, m.id, e.id, e.name");

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return Match.ParseMatches(result);
        }

        public static List<User> GetAllUsersInMatch()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT userid " +
                                            "FROM MatchLobby m " +
                                            "RIGHT JOIN MatchProgress mp ON m.id = mp.matchid " +
                                            "WHERE m.state != 2 ");

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return User.ParseUserIds(result);
        }

        public static bool SetPlayState(int matchid, MatchState state)
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

        public static int AddMatch(int episodeid, User user)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO MatchLobby (episodeid, creatorid) VALUES (@episodeid, @creatorid)");

            MySqlParameter episodeId = new MySqlParameter("@episodeid", MySqlDbType.Int32, 255);
            MySqlParameter creatorId = new MySqlParameter("@creatorid", MySqlDbType.Int32, 0);

            episodeId.Value = episodeid;
            creatorId.Value = user.Id;

            cmd.Parameters.Add(episodeId);
            cmd.Parameters.Add(creatorId);

            return DBHandler.InsertAndGet(cmd);
        }

        public static bool AddMatchProgress(int matchid, User user)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO MatchProgress (matchid, userid, startdate) " +
                                            "VALUES (@matchid, @userid, @startdate)");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 0);
            MySqlParameter startDate = new MySqlParameter("@startdate", MySqlDbType.VarChar, 0);

            matchId.Value = matchid;
            userId.Value = user.Id;
            startDate.Value = DateTime.Now.ToString("HH:mm dd-MM-yyyy");

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(userId);
            cmd.Parameters.Add(startDate);

            return DBHandler.Query(cmd);
        }

        public static Match GetMatchById(int matchId)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT m.id, m.state, u.id, u.username, e.id, e.name " +
                "FROM MatchLobby m " +
                "LEFT JOIN User u ON m.creatorid = u.id " +
                "LEFT JOIN Episode e ON e.id = m.episodeid " +
                "WHERE m.id = @matchid");

            MySqlParameter id = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            id.Value = matchId;
            cmd.Parameters.Add(id);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return Match.ParseMatch(result);
        }

        public static int CheckIfMatchExists(int matchId)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(id) " +
                                                "FROM MatchLobby m " +
                                                "WHERE m.id = @matchid");

            MySqlParameter id = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            id.Value = matchId;
            cmd.Parameters.Add(id);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return int.Parse(result[0][0]);
        }

        public static List<MatchProgress> GetMatchProgress(int matchId)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT u.id, u.username, mp.progress, mp.score, mp.mistakes, mp.lettersperminute, mp.time " +
             "FROM MatchProgress mp " +
             "LEFT JOIN MatchLobby m ON mp.matchid = m.id " +
             "LEFT JOIN Episode e ON m.episodeid = e.id " +
             "LEFT JOIN User u ON mp.userid = u.id " +
             "WHERE mp.matchid = @matchid");

            MySqlParameter id = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            id.Value = matchId;
            cmd.Parameters.Add(id);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return MatchProgress.ParseMatchProgress(result);
        }

        public static bool RemoveUserInMatch(int matchid, User user)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM MatchProgress WHERE matchid = @matchid AND userid = @userid");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            matchId.Value = matchid;
            userId.Value = user.Id;

            cmd.Parameters.Add(matchId);
            cmd.Parameters.Add(userId);

            return DBHandler.Query(cmd);
        }

        public static bool DeleteMatch(int matchid)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM MatchLobby WHERE id = @matchid");

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

        public static List<MatchProgress> GetScoresOrderByHighest(int matchid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT u.username, mp.score " +
            "FROM MatchProgress mp " +
            "LEFT JOIN User u ON mp.userid = u.id " +
            "WHERE mp.matchid = @matchid " +
            "ORDER BY mp.score DESC LIMIT 3");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);

            matchId.Value = matchid;

            cmd.Parameters.Add(matchId);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return MatchProgress.ParseSimpleProgress(result);
        }

        public static List<List<string>> GetMatchProgressesWithUser(int userid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT matchid, startdate, score, mistakes, lettersperminute, time FROM MatchProgress WHERE progress = 100 AND userid = @userid ORDER BY id DESC LIMIT 10");

            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            userId.Value = userid;
            cmd.Parameters.Add(userId);

            List<List<string>> result = DBHandler.SelectQuery(cmd);
            return result;
        }

        public static List<List<string>> GetAllScoresOrderByHighest(int matchid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT ep.name, u.username, mp.score, mp.mistakes, mp.lettersperminute, mp.time " +
            "FROM Episode ep " +
            "LEFT JOIN MatchLobby ml ON ml.episodeid = ep.id " +
            "LEFT JOIN MatchProgress mp ON ml.id = mp.matchid " +
            "LEFT JOIN User u ON mp.userid = u.id " +
            "WHERE mp.matchid = @matchid " +
            "ORDER BY mp.score DESC");

            MySqlParameter matchId = new MySqlParameter("@matchid", MySqlDbType.Int32, 255);
            matchId.Value = matchid;
            cmd.Parameters.Add(matchId);

            return DBHandler.SelectQuery(cmd);
        }
      
        public static bool UpdateAudioSetting(int userid, bool state)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE UserSettings set audio = @state WHERE userid = @userid ");

            MySqlParameter q_userid = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            MySqlParameter q_state = new MySqlParameter("@state", MySqlDbType.Int32, 255);

            q_userid.Value = userid;
            q_state.Value = state;

            cmd.Parameters.Add(q_userid);
            cmd.Parameters.Add(q_state);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateDyslecticSettings(int userid, bool dyslectic)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE UserSettings SET dyslectic = @dyslectic WHERE userid = @userid");

            MySqlParameter dyslecticParam = new MySqlParameter("@dyslectic", MySqlDbType.Int32, 255);
            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            dyslecticParam.Value = dyslectic;
            userId.Value = userid;

            cmd.Parameters.Add(userId);
            cmd.Parameters.Add(dyslecticParam);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateDefaultTheme(User user, string theme)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE UserSettings SET theme = @theme WHERE userid = @userid");

            MySqlParameter defaultTheme = new MySqlParameter("@theme", MySqlDbType.VarChar, 255);
            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 0);

            defaultTheme.Value = theme;
            userId.Value = user.Id;

            cmd.Parameters.Add(userId);
            cmd.Parameters.Add(defaultTheme);

            return DBHandler.Query(cmd);
        }

        public static List<string> GetAllGamemodeScores(int userid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT infinitemode, 3lifesmode, 1lifemode " +
            "FROM GamemodeResult " +
            "WHERE userid = @userid " +
            "LIMIT 1");

            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            userId.Value = userid;
            cmd.Parameters.Add(userId);

            return DBHandler.SelectQuery(cmd)[0];
        }

        public static int GetTotalEpisodeAmount()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(id) FROM Episode");

            return int.Parse(DBHandler.SelectQuery(cmd)[0][0]);
        }

        public static bool UpdateScoreInfiniteMode(int userid, int score)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE GamemodeResult set infinitemode = @score WHERE userid = @userid AND infinitemode < @score");

            MySqlParameter q_userid = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            MySqlParameter q_score = new MySqlParameter("@score", MySqlDbType.Int32, 255);

            q_userid.Value = userid;
            q_score.Value = score;

            cmd.Parameters.Add(q_userid);
            cmd.Parameters.Add(q_score);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateScore3LifesMode(int userid, int score)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE GamemodeResult set 3lifesmode = @score WHERE userid = @userid AND 3lifesmode < @score");

            MySqlParameter q_userid = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            MySqlParameter q_score = new MySqlParameter("@score", MySqlDbType.Int32, 255);

            q_userid.Value = userid;
            q_score.Value = score;

            cmd.Parameters.Add(q_userid);
            cmd.Parameters.Add(q_score);

            return DBHandler.Query(cmd);
        }

        public static bool UpdateScore1LifeMode(int userid, int score)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE GamemodeResult set 1lifemode = @score WHERE userid = @userid AND 1lifemode < @score");

            MySqlParameter q_userid = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            MySqlParameter q_score = new MySqlParameter("@score", MySqlDbType.Int32, 255);

            q_userid.Value = userid;
            q_score.Value = score;

            cmd.Parameters.Add(q_userid);
            cmd.Parameters.Add(q_score);

            return DBHandler.Query(cmd);
        }

        public static bool DeleteUserAccount(User user)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE User SET username = 'Deleted', email = NULL, password = NULL, salt = NULL WHERE id = @userid;");

            MySqlParameter userId = new MySqlParameter("@userid", MySqlDbType.Int32, 255);
            userId.Value = user.Id;
            cmd.Parameters.Add(userId);

            return DBHandler.Query(cmd);
        }
    }
}
