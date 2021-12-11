using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC = Controller.EpisodeController;

namespace Controller
{
    public static class MatchController
    {
        private static int _currentMatchId;
        private static string _creatorId;
        private static int _amountOfPlayers;
        private static List<List<string>> _matchInfo;
        private static List<string> _winners;
        private static List<string> _scores;

        public static List<List<string>> OpponentData { get; private set; } = new List<List<string>>();

        public static event EventHandler Refresh;

        public static string Winner1 { get; private set; }
        public static string Winner2 { get; private set; }
        public static string Winner3 { get; private set; }

        public static string Score1 { get; private set; }
        public static string Score2 { get; private set; }
        public static string Score3 { get; private set; }

        public static void Start() => EC.Start();

        public static void StartGame()
        {
            Episode episode = EC.ParseEpisode(int.Parse(_matchInfo[0][9]));
            EC.Initialise(episode, true);
            MultiplayerFetch();
            NavigationController.NavigateToPage(Pages.MatchPlayingPage);
        }

        public static void OnEpisodeFinished(object sender, EventArgs e)
        {
            FinishMatch();
            EC.EpisodeFinished -= OnEpisodeFinished;
            DBQueries.SetPlayState(int.Parse(_matchInfo[0][0]), 2);
            NavigationController.NavigateToPage(Pages.MatchResultPage);
        }

        public static void SetWinners()
        {
            List<List<string>> players = DBQueries.GetScoresOrderByHighest(_currentMatchId);
            _winners = players.Select(p => p[0]).ToList();
            _scores = players.Select(p => p[1]).ToList();

            //simple null check for now, suggestions for improvement welcome.
            Winner1 = _winners.Count > 0 ? _winners[0] : null;
            Winner2 = _winners.Count > 1 ? _winners[1] : null;
            Winner3 = _winners.Count > 2 ? _winners[2] : null;

            Score1 = _scores.Count > 0 ? _scores[0] : null;
            Score2 = _scores.Count > 1 ? _scores[1] : null;
            Score3 = _scores.Count > 2 ? _scores[2] : null;

            Refresh?.Invoke(null, new EventArgs());
        }

        public static void FinishMatch()
        {
            EC.StopAndSetEpisodeResult();
            UList student = (UList)Session.Get("student");

            int userId = student.Get<int>(0);
            int matchId = (int)Session.Get("matchId");

            DBQueries.SaveMatchResult(EC.CurrentEpisodeResult, matchId, userId);

            SetWinners();
        }

        public static void MultiplayerFetch()
        {
            // PUSH THIS CLIENT'S PROGRESS
            int progress_percent = (EC.LettersTyped * 100) / EC.CurrentEpisodeResult.MaxScore;
            int user_id = ((UList)Session.Get("student")).Get<int>(0);
            int match_id = _currentMatchId;
            DBQueries.UpdateMatchProgress(progress_percent, user_id, match_id);

            // FETCH OTHERS PROGRESS
            OpponentData.Clear();
            OpponentData = DBQueries.GetOpponentProgress(user_id, match_id);
        }

        /// <summary>
        /// <para>This method checks if there is already a user in a match.</para>
        /// It does this by running an sql query to get all user that are in a match and compare that to the user that is in the session (the logged in user)
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfUserExists()
        {
            UList student = (UList)Session.Get("student");
            return DBQueries.GetAllUsersInMatch().SelectMany(result => result.Where(data => data.Equals(student.Get<string>(0), StringComparison.Ordinal)).Select(data => new { })).Any();
        }
        /// <summary>
        /// <para>This method will make a match by adding a Match, and a MatchProgress to the database</para>
        /// It does this by getting the id of the just inserted Match and inserting it into MatchProgress with the current user.
        /// </summary>
        /// <param name="selectedValue"></param>
        public static void MakeMatch(int selectedValue)
        {
            UList student = (UList)Session.Get("student");
            _currentMatchId = DBQueries.AddMatch(selectedValue, student);
            DBQueries.AddMatchProgress(_currentMatchId, student);
        }

        /// <summary>
        /// <para>This method will add a user in MatchProgress to the database</para>
        /// It does this by getting the id of the chosen Match and inserting it in MatchProgressr.
        /// </summary>
        public static void AddUserInMatchProgress(string selectedValue)
        {
            _currentMatchId = int.Parse(selectedValue);
            DBQueries.AddMatchProgress(_currentMatchId, (UList)Session.Get("student"));
        }

        /// <summary>
        /// <para>This method will remove a user in MatchProgress to the database</para>
        /// It does this by giving the Match and User id to the method to delete the MatchProgress.
        /// </summary>
        public static void RemoveUserInMatchProgress()
        {
            DBQueries.RemoveUserInMatch(_currentMatchId, (UList)Session.Get("student"));
        }

        /// <summary>
        /// <para>This method get current MatchProgressInfo</para>
        /// It does this by giving the Match id to get info from MatchProgress.
        /// </summary>
        public static List<List<string>> GetMatchProgressInfo()
        {
            _matchInfo = DBQueries.GetMatchProgress(_currentMatchId);
            _amountOfPlayers = _matchInfo.Count();
            if (_amountOfPlayers != 0) _creatorId = _matchInfo[0][8];
            return _matchInfo;
        }

        /// <summary>
        /// <para>This method will update in Match a new Userid, who joined the match, as creator</para>
        /// It does this by giving the Matchid and the id of the user who first joined after the current creator.
        /// </summary>
        public static void UpdateCreatorInMatch()
        {
            DBQueries.UpdateNewCreatorInMatch(_currentMatchId, 7); // not done yet
            RemoveUserInMatchProgress();
        }

        public static bool CheckUserIsCreator()
        {
            UList student = (UList)Session.Get("student");
            return student.Get<string>(0).Equals(_creatorId, StringComparison.Ordinal);
        }

        public static void DeleteMatch()
        {
            DBQueries.RemoveUserInMatch(_currentMatchId, (UList)Session.Get("student"));
            DBQueries.DeleteMatch(_currentMatchId);
        }

        public static bool CheckCreatorIsAloneInMatch() { return _amountOfPlayers == 1; }

        public static int GetMatchId() { return _currentMatchId; }
    }
}
