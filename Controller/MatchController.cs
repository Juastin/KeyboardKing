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

        private static Episode _currentEpisode;
        public static EpisodeStep CurrentEpisodeStep { get; private set; }
        public static EpisodeResult CurrentEpisodeResult { get; private set; }
        public static int LettersTyped { get; private set; }
        private static int _wordIndex;
        private static int _wrongIndex;

        private static DateTime _startTime;

        public static event EventHandler WordChanged;
        public static event EventHandler EpisodeFinished;

        public static string Word { get => CurrentEpisodeStep?.Word; }

        public static int Points { get; set; }
        public static string WordOverlayCorrect { get => CurrentEpisodeStep?.Word.Substring(0, _wordIndex); }
        public static string WordOverlayWrong { get => CurrentEpisodeStep?.Word.Substring(0, _wrongIndex); }

        public static void Start()
        {
            _startTime = DateTime.Now;
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

        public static void Initialise(Episode episode)
        {
            _currentEpisode = episode;
            CurrentEpisodeResult = new EpisodeResult();
            _startTime = new DateTime();
            _wordIndex = 0;
            _wrongIndex = 0;
            LettersTyped = 0;
            CurrentEpisodeResult.MaxScore = CalculateMaxScore(episode);
            NextEpisodeStep();
        }


        /// <summary>
        /// Tries to dequeue next episode step from the current episode.
        /// </summary>
        private static void NextEpisodeStep()
        {
            _wordIndex = 0;
            _wrongIndex = 0;
            if (_currentEpisode.EpisodeSteps.TryDequeue(out EpisodeStep step))
            {
                CurrentEpisodeStep = step;
            }
            else
            {
                FinishEpisode();
            }
            WordChanged?.Invoke(null, new EventArgs());
        }

        /// <summary>
        /// <para>Processes the given bool.</para>
        /// Will call <see cref="NextEpisodeStep"/> once the word has been correctly and fully typed.
        /// </summary>
        /// <param name="isCorrect"></param>
        private static void NextLetter(bool isCorrect)
        {
            if (isCorrect)
            {
                _wordIndex++;
                LettersTyped++;
            }

            else
            {
                _wrongIndex = _wordIndex + 1;
                CurrentEpisodeResult.Mistakes++;
            }

            if (_wordIndex >= CurrentEpisodeStep.Word.Length)
            {
                NextEpisodeStep();
                Points = Points + 10;
            }
            else
            {
                WordChanged?.Invoke(null, new EventArgs());
            }
        }

        /// <summary>
        /// Creates a new Episode object with all corresponding EpisodeSteps
        /// </summary>
        /// <param name="episodeId">episode id from the requested episode</param>
        /// <returns></returns>
        public static Episode ParseEpisode(int episodeId)
        {
            List<List<string>> results = DBQueries.GetAllEpisodeStepsFromEpisode(episodeId);

            Session.Add("matchId", _currentMatchId);

            Episode episode = new Episode();

            foreach (List<string> word in results)
            {
                EpisodeStep es = new EpisodeStep() { Word = word[0] };
                episode.EpisodeSteps.Enqueue(es);
            }
            return episode;
        }

        /// <summary>
        /// Checks user input and parses the result to <see cref="NextLetter(bool)"/>
        /// </summary>
        /// <param name="input">Input from the user</param>
        public static void CheckInput(char input)
        {
            NextLetter(CurrentEpisodeStep.Word[_wordIndex].Equals(input));
        }

        public static void FinishEpisode()
        {
            CurrentEpisodeResult.Time = CalculateTime(_startTime);
            CurrentEpisodeResult.Score = CalculateScore(CurrentEpisodeResult.MaxScore, CurrentEpisodeResult.Mistakes);
            CurrentEpisodeResult.LettersPerMinute = CalculateLetterPerMinute(CurrentEpisodeResult.Time, CurrentEpisodeResult.MaxScore);

            UList student = (UList)Session.Get("student");

            int userId = student.Get<int>(0);
            int matchId = (int)Session.Get("matchId");

            DBQueries.SaveMatchResult(CurrentEpisodeResult, matchId, userId);

            EpisodeFinished?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Returns the time difference between the given parameter and the time of now.
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public static TimeSpan CalculateTime(DateTime startTime)
        {
            return DateTime.Now - startTime;
        }
        /// <summary>
        /// Returns a percentage of the correct typed letters based on the mistakes and the max amount of letters.
        /// </summary>
        /// <param name="maxScore"></param>
        /// <param name="mistakes"></param>
        /// <returns></returns>
        public static int CalculateScore(int maxScore, int mistakes)
        {
            return (int)((double)(maxScore - mistakes) / maxScore * 100);
        }
        /// <summary>
        /// Calculates the max amount of store possible by getting the total amount of letters that can be written.
        /// </summary>
        /// <param name="episode"></param>
        /// <returns></returns>
        public static int CalculateMaxScore(Episode episode)
        {
            return episode.EpisodeSteps.Sum(episodeStep => episodeStep.Word.Length);
        }
        /// <summary>
        /// Calculates the letters that are written per minute by dividing the amount of letters possible by the time spend on the episode.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="letters"></param>
        /// <returns></returns>
        public static double CalculateLetterPerMinute(TimeSpan time, int letters)
        {
            return Math.Round(letters / time.TotalMinutes);
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
            List<List<string>> matchInfo = DBQueries.GetMatchProgress(_currentMatchId);
            _amountOfPlayers = matchInfo.Count();
            if (_amountOfPlayers != 0) _creatorId = matchInfo[0][8];
            return matchInfo;
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