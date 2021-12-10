using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class MatchController
    {
        private static int _currentMatchId;
        private static string _creatorId;
        private static int _amountOfPlayers;
        private static List<List<string>> _matchInfo;

        public static EpisodeStep CurrentEpisodeStep { get; private set; }
        public static EpisodeResult CurrentEpisodeResult { get; private set; }
        public static int LettersTyped { get => EpisodeController.LettersTyped; }
        public static List<List<string>> OpponentData { get; private set; } = new List<List<string>>();

        private static DateTime _startTime;

        public static event EventHandler Refresh;
        public static string Word { get => CurrentEpisodeStep?.Word; }

        private static List<string> _winnaars;
        private static List<string> _scores;

        public static string WordOverlayCorrect { get => EpisodeController.WordOverlayCorrect; }
        public static string WordOverlayWrong { get => EpisodeController.WordOverlayWrong; }

        public static string Winnaar1 { get; private set; }
        public static string Winnaar2 { get; private set; }
        public static string Winnaar3 { get; private set; }

        public static string Score1 { get; private set; }
        public static string Score2 { get; private set; }
        public static string Score3 { get; private set; }

        public static void Start()
        {
            _startTime = DateTime.Now;
        }

        public static void StartGame()
        {
            Episode episode = EpisodeController.ParseEpisode(int.Parse(_matchInfo[0][9]));
            EpisodeController.Initialise(episode, true);
            NavigationController.NavigateToPage(Pages.MatchPlayingPage);
        }

        public static void OnEpisodeFinished(object sender, EventArgs e)
        {
            FinishMatch();
            EpisodeController.EpisodeFinished -= OnEpisodeFinished;
            DBQueries.SetPlayState(int.Parse(_matchInfo[0][0]), 2);
            NavigationController.NavigateToPage(Pages.MatchResultPage);
        }

        public static void SetWinners()
        {
            List<List<string>> players = DBQueries.GetScoresOrderByHighest(_currentMatchId);
            _winnaars = players.Select(p => p[0]).ToList();
            _scores = players.Select(p => p[1]).ToList();

            Winnaar1 = _winnaars[0];
            Winnaar2 = _winnaars[1];
            Winnaar3 = _winnaars[2];

            Score1 = _scores[0];
            Score2 = _scores[1];
            Score3 = _scores[2];

            Refresh?.Invoke(null, new EventArgs());
        }

        public static void FinishMatch()
        {
            EpisodeController.CurrentEpisodeResult.Time = EpisodeController.CalculateTime(_startTime);
            EpisodeController.CurrentEpisodeResult.Score = EpisodeController.CalculateScore(EpisodeController.CalculateLetterPerMinute(CurrentEpisodeResult.Time, CurrentEpisodeResult.MaxScore));
            EpisodeController.CurrentEpisodeResult.ScorePercentage = EpisodeController.CalculatePercentage(CurrentEpisodeResult.MaxScore, CurrentEpisodeResult.Mistakes);
            EpisodeController.CurrentEpisodeResult.LettersPerMinute = EpisodeController.CalculateLetterPerMinute(CurrentEpisodeResult.Time, CurrentEpisodeResult.MaxScore);
         
            UList student = (UList)Session.Get("student");

            int userId = student.Get<int>(0);
            int matchId = (int)Session.Get("matchId");

            DBQueries.SaveMatchResult(EpisodeController.CurrentEpisodeResult, matchId, userId);

            SetWinners();
        }

        public static void MultiplayerFetch()
        {
            // PUSH THIS CLIENT'S PROGRESS
            int progress_percent = (LettersTyped * 100) / EpisodeController.CurrentEpisodeResult.MaxScore;
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
