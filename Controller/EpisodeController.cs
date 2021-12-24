using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatabaseController;
using Model;

namespace Controller
{
    /// <summary>
    /// Class <c>EpisodeController</c> handles and checks the user input compared to the current episode.
    /// </summary>
    public static class EpisodeController
    {
        private static Episode _currentEpisode;
        public static List<Chapter> Chapters { get; private set; }
        public static EpisodeStep CurrentEpisodeStep { get; private set; }
        public static EpisodeResult CurrentEpisodeResult { get; private set; }
        public static int LettersTyped { get; private set; }
        private static int _wordIndex;
        private static int _wrongIndex;

        private static Stopwatch _stopwatch;

        public static event EventHandler WordChanged;
        public static event EventHandler EpisodeFinished;
        public static event EventHandler EpisodeResultUpdated;

        public static string Word { get => CurrentEpisodeStep?.Word; }

        public static int Difficulty { get; set; }

        private static bool _repeatMistake;

        public static string WordOverlayCorrect { get =>CurrentEpisodeStep?.Word.Substring(0, _wordIndex); }
        public static string WordOverlay { get => CurrentEpisodeStep?.Word.Substring(_wordIndex); }
        public static string WordOverlayWrong { get =>CurrentEpisodeStep?.Word.Substring(_wordIndex, _wrongIndex); }
        public static bool IsStarted { get; private set; }

        public static int Coins { get; private set; }

      
        public static void Start()
        {
            _stopwatch.Start();
            IsStarted = true;
        }

        public static void Pause()
        {
            _stopwatch.Stop();
            MessageController.ShowPause(Pages.PausePage, "De episode is gepauzeerd.", Pages.EpisodePage);
        }

        public static void Exit()
        {
            MessageController.ShowConfirmation(Pages.ConfirmationPage, "Weet je zeker dat je de episode wilt afsluiten?", Pages.PausePage, Pages.ChaptersPage);
        }

        /// <summary>
        /// <para>
        ///     <c>Initialise</c> needs to be called before starting an episode.  
        /// </para>
        /// Initializes all variables needed to play and keep track of the episode.
        /// </summary>
        /// <param name="episode">The episode that is going to be played</param>
        public static void Initialise(Episode episode, bool isMatch)
        {
            _currentEpisode = episode;
            CurrentEpisodeResult = new EpisodeResult();
            _stopwatch = new Stopwatch();
            _repeatMistake = false;
            Difficulty = 30;
            _wordIndex = 0;
            _wrongIndex = 0;
            LettersTyped = 0;
            Coins = 0;
            CurrentEpisodeResult.MaxScore = CalculateTotalLetters(episode);
            NextEpisodeStep();

            if (isMatch)
                EpisodeFinished += MatchController.OnEpisodeFinished;
            else
                EpisodeFinished += OnEpisodeFinished;
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
                EpisodeFinished?.Invoke(null, EventArgs.Empty);
                EpisodeFinished -= OnEpisodeFinished;
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
                _wrongIndex = 0;
                _repeatMistake = false;
            }

            else
            {
                _wrongIndex = 1;
                if (_repeatMistake == false)
                {
                    CurrentEpisodeResult.Mistakes++;
                    _repeatMistake = true;
                }
            }

            if (_wordIndex >= CurrentEpisodeStep.Word.Length)
            {
                NextEpisodeStep();
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
            List<EpisodeStep> steps = DBQueries.GetAllEpisodeStepsFromEpisode(episodeId);

            Session.Remove("episodeId");
            Session.Add("episodeId", episodeId);

            Episode episode = new Episode();
            steps.ForEach(s => episode.EpisodeSteps.Enqueue(s));


            return episode;
        }

        public static List<Chapter> RetrieveChapters()
        {
            User user = (User)Session.Get("student");
            Chapters = DBQueries.GetAllChapters(user);
            return Chapters;
        }

        /// <summary>
        /// Checks user input and parses the result to <see cref="NextLetter(bool)"/>
        /// </summary>
        /// <param name="input">Input from the user</param>
        public static void CheckInput(char input)
        {
            NextLetter(CurrentEpisodeStep.Word[_wordIndex].Equals(input));
        }
        public static string GetTimeFormat()
        {
            return _stopwatch?.Elapsed.ToString("mm\\:ss");
        }

        public static void StopAndSetEpisodeResult()
        {
            _stopwatch.Stop();
            IsStarted = false;
            CurrentEpisodeResult.Time = _stopwatch.Elapsed;
            CurrentEpisodeResult.Accuracy = CalculateAccuracy(CurrentEpisodeResult.MaxScore, CurrentEpisodeResult.Mistakes);
            CurrentEpisodeResult.LettersPerMinute = CalculateLetterPerMinute(CurrentEpisodeResult.Time, CurrentEpisodeResult.MaxScore);
            CurrentEpisodeResult.Score = (int)CalculateScore(CurrentEpisodeResult.LettersPerMinute, CurrentEpisodeResult.MaxScore, CurrentEpisodeResult.Mistakes);
            CurrentEpisodeResult.Passed = CheckIfPassedEpisode();
        }

        public static void OnEpisodeFinished(object sender, EventArgs e)
        {
            StopAndSetEpisodeResult();
           
            User student = (User)Session.Get("student");
            int episodeId = (int)Session.Get("episodeId");
            GiveCoins(student, episodeId);
            DBQueries.SaveResult(CurrentEpisodeResult, episodeId, student.Id);

            EpisodeResultUpdated?.Invoke(null, EventArgs.Empty);
            NavigationController.NavigateToPage(Pages.EpisodeResultPage);
        }

        private static void GiveCoins(User student, int episodeId)
        {
            int highscore = DBQueries.GetHighscoreEpisode(student, episodeId); //5550

            if(CurrentEpisodeResult.Score > highscore)
                Coins = (CurrentEpisodeResult.Score - highscore) / 100;

            DBQueries.UpdateCoins(Coins, student);

            student.Coins = DBQueries.GetCoinsOfUser(student);
            Session.Add("student", student);
        }
        
        public static int GetCoins(User student)
        {
            return DBQueries.GetCoinsOfUser(student);
        }

        /// <summary>
        /// Returns a percentage of the correct typed letters, accuracy, based on the mistakes and the max amount of letters.
        /// </summary>
        /// <param name="maxScore"></param>
        /// <param name="mistakes"></param>
        /// <returns></returns>
        public static int CalculateAccuracy(int maxScore, int mistakes)
        {
            return (int)((double)(maxScore - mistakes) / maxScore * 100);
        }

        /// <summary>
        /// Returns the score based on the point in earned in the episode * how fast you type.
        /// </summary>
        /// <param name="LettersPerMinute"></param>
        /// <returns></returns>
        public static double CalculateScore(double LettersPerMinute, int totalLetters, int mistakes)
        {
            double score = LettersPerMinute * (totalLetters * 10 - mistakes * Difficulty) / (400 * totalLetters * 10) * 10000;
            return score >= 0 ? score : 0;
        }

        /// <summary>
        /// Calculates the max amount of store possible by getting the total amount of letters that can be written.
        /// </summary>
        /// <param name="episode"></param>
        /// <returns></returns>
        public static int CalculateTotalLetters(Episode episode)
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

        public static bool CheckIfPassedEpisode()
        {
            int threshold = DBQueries.Getpassthreshold((int)Session.Get("episodeId"));
            return CurrentEpisodeResult.Accuracy >= threshold;
        }

        public static int CalculateNumberOfNotDone()
        {
            return CurrentEpisodeResult.MaxScore - LettersTyped + CurrentEpisodeResult.Mistakes;
        }

        public static void ForcedStopAndSetEpisodeResult()
        {
            _stopwatch.Stop();
            IsStarted = false;
            CurrentEpisodeResult.Time = _stopwatch.Elapsed;
            int totalMistakes = CalculateNumberOfNotDone();

            CurrentEpisodeResult.Accuracy = CalculateAccuracy(CurrentEpisodeResult.MaxScore, totalMistakes);
            CurrentEpisodeResult.LettersPerMinute = CalculateLetterPerMinute(CurrentEpisodeResult.Time, CurrentEpisodeResult.MaxScore);
            CurrentEpisodeResult.Score = (int)CalculateScore(CurrentEpisodeResult.LettersPerMinute, CurrentEpisodeResult.MaxScore, totalMistakes);
            CurrentEpisodeResult.Passed = CheckIfPassedEpisode();
        }

    }
}
