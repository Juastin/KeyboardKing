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
        private static int _creatorId;
        private static int _amountOfPlayers;
        private static List<MatchProgress> _matchInfo;

        public static List<MatchProgress> OpponentData { get; private set; } = new List<MatchProgress>();

        public static event EventHandler Refresh;

        public static string Winner1 { get; private set; }
        public static string Winner2 { get; private set; }
        public static string Winner3 { get; private set; }

        public static int Score1 { get; private set; }
        public static int Score2 { get; private set; }
        public static int Score3 { get; private set; }

        public static void Start() => EC.Start();

        public static void StartGame()
        {
            Episode episode = EC.ParseEpisode(_matchInfo[0].EpisodeId);
            EC.Initialise(episode, true);
            MultiplayerFetch();
            NavigationController.NavigateToPage(Pages.MatchPlayingPage);
        }

        public static void OnEpisodeFinished(object sender, EventArgs e)
        {
            FinishMatch();
            EC.EpisodeFinished -= OnEpisodeFinished;
            DBQueries.SetPlayState(_matchInfo[0].MatchId, MatchState.Finished);
            NavigationController.NavigateToPage(Pages.MatchWaitingResultPage);
        }

        public static void SetWinners()
        {
            List<MatchProgress> players = DBQueries.GetScoresOrderByHighest(_currentMatchId);

            //simple null check for now, suggestions for improvement welcome.
            Winner1 = players.Count > 0 ? players[0].Username : null;
            Winner2 = players.Count > 1 ? players[1].Username : null;
            Winner3 = players.Count > 2 ? players[2].Username : null;

            Score1 = players.Count > 0 ? players[0].Progress : default;
            Score2 = players.Count > 1 ? players[1].Progress : default;
            Score3 = players.Count > 2 ? players[2].Progress : default;

            Refresh?.Invoke(null, new EventArgs());
        }

        public static void FinishMatch()
        {
            EC.StopAndSetEpisodeResult();
            User student = (User)Session.Get("student");

            DBQueries.SaveMatchResult(EC.CurrentEpisodeResult, _currentMatchId, student.Id);
            SetWinners();
        }

        public static void MultiplayerFetch()
        {
            // PUSH THIS CLIENT'S PROGRESS
            int progress_percent = EC.LettersTyped * 100 / EC.CurrentEpisodeResult.MaxScore;
            User student = (User)Session.Get("student");
            int match_id = _currentMatchId;
            DBQueries.UpdateMatchProgress(progress_percent, student.Id, match_id);

            // FETCH OTHERS PROGRESS
            OpponentData.Clear();
            OpponentData = DBQueries.GetOpponentProgress(student.Id, match_id);
        }

        /// <summary>
        /// <para>This method checks if there is already a user in a match.</para>
        /// It does this by running an sql query to get all user that are in a match and compare that to the user that is in the session (the logged in user)
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfUserExists()
        {
            User student = (User)Session.Get("student");
            return DBQueries.GetAllUsersInMatch().Where(user => user.Id == student.Id).Count() > 0;
        }
        /// <summary>
        /// <para>This method will make a match by adding a Match, and a MatchProgress to the database</para>
        /// It does this by getting the id of the just inserted Match and inserting it into MatchProgress with the current user.
        /// </summary>
        /// <param name="selectedValue"></param>
        public static void MakeMatch(int selectedValue)
        {
            User student = (User)Session.Get("student");
            _currentMatchId = DBQueries.AddMatch(selectedValue, student);
            DBQueries.AddMatchProgress(_currentMatchId, student);
        }

        /// <summary>
        /// <para>This method will add a user in MatchProgress to the database</para>
        /// It does this by getting the id of the chosen Match and inserting it in MatchProgressr.
        /// </summary>
        public static void AddUserInMatchProgress(int matchId)
        {
            _currentMatchId = matchId;
            DBQueries.AddMatchProgress(_currentMatchId, (User)Session.Get("student"));
        }

        /// <summary>
        /// <para>This method will remove a user in MatchProgress to the database</para>
        /// It does this by giving the Match and User id to the method to delete the MatchProgress.
        /// </summary>
        public static void RemoveUserInMatchProgress() => DBQueries.RemoveUserInMatch(_currentMatchId, (User)Session.Get("student"));

        /// <summary>
        /// <para>This method get current MatchProgressInfo</para>
        /// It does this by giving the Match id to get info from MatchProgress.
        /// </summary>
        public static List<MatchProgress> GetMatchProgressInfo()
        {
            _matchInfo = DBQueries.GetMatchProgress(_currentMatchId);
            _amountOfPlayers = _matchInfo.Count();
            if (_amountOfPlayers != 0)
                _creatorId = _matchInfo[0].HostId;
            return _matchInfo;
        }

        /// <summary>
        /// <para>This method will update in Match a new Userid, who joined the match, as creator</para>
        /// It does this by giving the Matchid and the id of the user who first joined after the current creator.
        /// </summary>
        public static void UpdateCreatorInMatch() => DBQueries.UpdateNewCreatorInMatch(_currentMatchId, _matchInfo[1].UserId);

        /// <summary>
        /// <para>This method will check if the user is the creator</para>
        /// It does this by checking if the userid is the same as the creatorid
        /// </summary>
        public static bool CheckUserIsCreator()
        {
            User student = (User)Session.Get("student");
            return student.Id == _creatorId;
        }

        public static bool CheckUserIsCreator(int userId)
        {
            return _creatorId == userId;
        }

        public static void DeleteMatch()
        {
            DBQueries.RemoveUserInMatch(_currentMatchId, (User)Session.Get("student"));
            DBQueries.DeleteMatch(_currentMatchId);
        }

        public static bool CheckCreatorIsAloneInMatch() { return _amountOfPlayers == 1; }

        public static bool CheckIfEverybodyDone()
        {
            List<MatchProgress> progress = DBQueries.GetAllProgress(_currentMatchId);
            return progress.Select(p => p.Progress).All(p => p == 100);
        }

        public static int GetMatchId() { return _currentMatchId; }

        public static void SetPlayingState()
        {
            DBQueries.SetPlayState(_currentMatchId, MatchState.Started);
        }
    }
}
