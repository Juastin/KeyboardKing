using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualBasic.CompilerServices;
using Model;

namespace Controller
{
    /// <summary>
    /// Class <c>EpisodeController</c> handles and checks the user input compared to the current episode.
    /// </summary>
    public static class EpisodeController
    {
        private static Episode _currentEpisode;
        public static EpisodeStep CurrentEpisodeStep { get; private set; }
        public static EpisodeResult CurrentEpisodeResult { get; private set; }
        public static int LettersTyped { get; private set; }
        private static int _wordIndex;
        private static int _wrongIndex;
        
        private static Stopwatch _stopwatch;

        public static event EventHandler WordChanged;
        public static event EventHandler EpisodeFinished;

        public static string Word { get => CurrentEpisodeStep?.Word; }

        public static int Points { get; set; }
        public static string WordOverlayCorrect { get =>CurrentEpisodeStep?.Word.Substring(0, _wordIndex); }
        public static string WordOverlayWrong { get =>CurrentEpisodeStep?.Word.Substring(0, _wrongIndex); }

        public static void Start()
        {
            _stopwatch.Start();
        }

        public static void Pause()
        {
            _stopwatch.Stop();
        }

        /// <summary>
        /// <para>
        ///     <c>Initialise</c> needs to be called before starting an episode.  
        /// </para>
        /// Initializes all variables needed to play and keep track of the episode.
        /// </summary>
        /// <param name="episode">The episode that is going to be played</param>
        public static void Initialise(Episode episode)
        {
            _currentEpisode = episode;
            CurrentEpisodeResult = new EpisodeResult();
            _stopwatch = new Stopwatch();
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
            if(_currentEpisode.EpisodeSteps.TryDequeue(out EpisodeStep step))
                CurrentEpisodeStep = step;
            else
                FinishEpisode();

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

            Session.Remove("episodeId");
            Session.Add("episodeId", episodeId);

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
            _stopwatch.Stop();
            CurrentEpisodeResult.Time = _stopwatch.Elapsed;
            CurrentEpisodeResult.Score = CalculateScore(CurrentEpisodeResult.MaxScore, CurrentEpisodeResult.Mistakes);
            CurrentEpisodeResult.LettersPerMinute = CalculateLetterPerMinute(CurrentEpisodeResult.Time, CurrentEpisodeResult.MaxScore);

            UList student = (UList)Session.Get("student");

            int userId = student.Get<int>(0);
            int episodeId = (int)Session.Get("episodeId");
            
            DBQueries.SaveResult(CurrentEpisodeResult, episodeId, userId);

            EpisodeFinished?.Invoke(null, EventArgs.Empty);
        }
        /// <summary>
        /// Returns a percentage of the correct typed letters based on the mistakes and the max amount of letters.
        /// </summary>
        /// <param name="maxScore"></param>
        /// <param name="mistakes"></param>
        /// <returns></returns>
        public static int CalculateScore(int maxScore, int mistakes)
        {
            return (int)((double)(maxScore-mistakes) / maxScore * 100);
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
    }
}
