using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using EC = Controller.EpisodeController;
using DatabaseController;

namespace Controller
{
    public static class MatchController
    {
        public static Match CurrentMatch { get; private set; }
        private static List<MatchProgress> _matchProgress;

        public static List<MatchProgress> OpponentData { 
            get 
            {
                return _matchProgress
                    .Where(p => p.User.Username != ((User)Session.Get("student")).Username)
                    .OrderByDescending(p => p.Score)
                    .Take(4)
                    .ToList();
            } 
        }

        public static event EventHandler Refresh;

        public static string Winner1 { get; private set; }
        public static string Winner2 { get; private set; }
        public static string Winner3 { get; private set; }

        public static int Score1 { get; private set; }
        public static int Score2 { get; private set; }
        public static int Score3 { get; private set; }

        /// <summary>
        /// This method initialise the match and episode 
        /// </summary>
        /// <param name="match"></param>
        public static void Initialize(Match match)
        {
            CurrentMatch = match;
            CurrentMatch.Episode = EC.ParseEpisode(match.Episode.Id);
            EC.Initialise(CurrentMatch.Episode, true);
        }

        /// <summary>
        /// This method starts the episode from the match
        /// </summary>
        public static void Start() => EC.Start();

        /// <summary>
        /// This method starts the match
        /// </summary>
        public static void StartGame()
        {
            MultiplayerFetch();
            NavigationController.NavigateToPage(Pages.MatchPlayingPage);
        }

        /// <summary>
        /// Method will be called when match is finished or is forced stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnEpisodeFinished(object sender, EventArgs e)
        {
            EC.EpisodeFinished -= OnEpisodeFinished;
            if (EC.IsFinished())
                EC.CurrentEpisodeResult.Mistakes = EC.CalculateNumberOfMistakes();

            SaveMatchProgressResult();
            NavigationController.NavigateToPage(Pages.MatchWaitingResultPage);
        }

        /// <summary>
        /// This method set the winners from the match by getting matchprogress
        /// </summary>
        public static void SetWinners()
        {
            List<MatchProgress> players = DBQueries.GetScoresOrderByHighest(CurrentMatch.Id);

            //simple null check for now, suggestions for improvement welcome.
            Winner1 = players.Count > 0 ? players[0].User.Username : null;
            Winner2 = players.Count > 1 ? players[1].User.Username : null;
            Winner3 = players.Count > 2 ? players[2].User.Username : null;

            Score1 = players.Count > 0 ? players[0].Progress : default;
            Score2 = players.Count > 1 ? players[1].Progress : default;
            Score3 = players.Count > 2 ? players[2].Progress : default;

            Refresh?.Invoke(null, new EventArgs());
        }

        /// <summary>
        /// Saves matchprogress result in database and set the current winners
        /// </summary>
        public static void SaveMatchProgressResult()
        {
            EC.StopAndSetEpisodeResult();
            User student = (User)Session.Get("student");

            DBQueries.SaveMatchResult(EC.CurrentEpisodeResult, CurrentMatch.Id, student.Id);
            SetWinners();

            Session.Add("MatchHistorySelectedMatch", CurrentMatch.Id);
        }


        /// <summary>
        /// update current matchprogress of user and gets matchprogress from current match
        /// </summary>
        public static void MultiplayerFetch()
        {
            // PUSH THIS CLIENT'S PROGRESS
            int progress_percent = EC.LettersTyped * 100 / EC.CurrentEpisodeResult.MaxScore;
            User student = (User)Session.Get("student");
            DBQueries.UpdateMatchProgress(progress_percent, student.Id, CurrentMatch.Id);

            // FETCH OTHERS PROGRESS
            _matchProgress = DBQueries.GetMatchProgress(CurrentMatch.Id);
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
        public static void MakeMatch(int episodeId)
        {
            User student = (User)Session.Get("student");
            int matchId = DBQueries.AddMatch(episodeId, student);
            Initialize(DBQueries.GetMatchById(matchId));
            DBQueries.AddMatchProgress(matchId, student);
        }

        /// <summary>
        /// This method will add a user in MatchProgress to the database
        /// It does this by getting the id of the chosen Match and inserting it in MatchProgressr.
        /// </summary>
        public static void AddUserInMatchProgress()
        {
            DBQueries.AddMatchProgress(CurrentMatch.Id, (User)Session.Get("student"));
        }

        /// <summary>
        /// This method will remove a user in MatchProgress to the database
        /// It does this by giving the Match and User id to the method to delete the MatchProgress.
        /// </summary>
        public static void RemoveUserInMatchProgress() => DBQueries.RemoveUserInMatch(CurrentMatch.Id, (User)Session.Get("student"));

        /// <summary>
        /// This method get current MatchProgressInfo
        /// It does this by giving the Match id to get info from MatchProgress.
        /// </summary>
        public static List<MatchProgress> GetMatchProgressInfo()
        {
            CurrentMatch = DBQueries.GetMatchById(CurrentMatch.Id);
            _matchProgress = DBQueries.GetMatchProgress(CurrentMatch.Id);
            return _matchProgress;
        }

        /// <summary>
        /// This method will update in Match a new Userid, who joined the match, as creator
        /// It does this by giving the Matchid and the id of the user who first joined after the current creator.
        /// </summary>
        public static void UpdateCreatorInMatch() => DBQueries.UpdateNewCreatorInMatch(CurrentMatch.Id, _matchProgress[1].User.Id);

        /// <summary>
        /// This method will check if the user is the creator
        /// It does this by checking if the userid is the same as the creatorid
        /// </summary>
        /// <returns></returns>
        public static bool CheckUserIsCreator()
        {
            User student = (User)Session.Get("student");
            return student.Id == CurrentMatch.Host.Id;
        }

        /// <summary>
        /// Checks is user is creator
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CheckUserIsCreator(int userId)
        {
            return CurrentMatch.Host.Id == userId;
        }

        /// <summary>
        /// Checks if the match still exists
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public static bool CheckIfMatchExists(int matchid)
        {
            return DBQueries.CheckIfMatchExists(matchid) > 0;
        }

        /// <summary>
        /// Deletes match and creator in matchprogress 
        /// </summary>
        public static void DeleteMatch()
        {
            DBQueries.RemoveUserInMatch(CurrentMatch.Id, (User)Session.Get("student"));
            DBQueries.DeleteMatch(CurrentMatch.Id);
        }

        /// <summary>
        /// Checks if the creator from the match is alone in the matchlobby
        /// </summary>
        /// <returns></returns>
        public static bool CheckCreatorIsAloneInMatch() { return _matchProgress.Count == 1; }

        /// <summary>
        /// Checks if all participants are done with playing
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfEverybodyDone()
        {
            GetMatchProgressInfo();
            return _matchProgress.Select(p => p.Progress).All(p => p == 100);
        }

        /// <summary>
        /// returns current match id
        /// </summary>
        /// <returns></returns>
        public static int GetMatchId() { return CurrentMatch.Id; }

        /// <summary>
        /// Set matchstate from match in database
        /// </summary>
        /// <param name="matchState"></param>
        public static void SetPlayingState(MatchState matchState)
        {
            DBQueries.SetPlayState(CurrentMatch.Id, matchState);
        }

        /// <summary>
        /// Checks if the match is forced stopped
        /// </summary>
        /// <returns></returns>
        public static bool CheckForcedStop()
        {
            GetMatchProgressInfo();
            return CurrentMatch.State == MatchState.Finished;
        }
    }
}
