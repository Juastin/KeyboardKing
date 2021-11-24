using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private static int _wordIndex;
        private static int _wrongIndex;
        private static DateTime _startTime;

        public static event EventHandler WordChanged;
        public static event EventHandler EpisodeFinished;

        public static string Word { get => CurrentEpisodeStep?.Word; }
        public static string WordOverlayCorrect { get =>CurrentEpisodeStep?.Word.Substring(0, _wordIndex); }
        public static string WordOverlayWrong { get =>CurrentEpisodeStep?.Word.Substring(0, _wrongIndex); }

        /// <summary>
        /// <para>
        ///     <c>Initilise</c> needs to be called before starting an episode.  
        /// </para>
        /// Initialises all variables needed to play and keep track of the episode.
        /// </summary>
        /// <param name="episode">The episode that is going to be played</param>
        public static void Initialise(Episode episode)
        {
            _currentEpisode = episode;
            CurrentEpisodeResult = new EpisodeResult();
            _startTime = new DateTime();
            _wordIndex = 0;
            _wrongIndex = 0;
            CurrentEpisodeResult.MaxScore = CalculateMaxScore(episode);
            NextEpisodeStep();
            _startTime = DateTime.Now;
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
        /// <para>Proccesses the given bool.</para>
        /// Will call <see cref="NextEpisodeStep"/> once the word has been correctly and fully typed.
        /// </summary>
        /// <param name="isCorrect"></param>
        private static void NextLetter(bool isCorrect)
        {
            if (isCorrect)
                _wordIndex++;
            else
            {
                _wrongIndex = _wordIndex + 1;
                CurrentEpisodeResult.Mistakes++;
            }
                
            if (_wordIndex >= CurrentEpisodeStep.Word.Length)
                NextEpisodeStep();
            else
                WordChanged?.Invoke(null, new EventArgs());
        }

        /// <summary>
        /// Creates a new Episode object with all corresponding EpisodeSteps
        /// </summary>
        /// <param name="episodeId">episode id from the requested episode</param>
        /// <returns></returns>
        public static Episode ParseEpisode(string episodeId)
        {
            List<List<string>> results = DBQueries.GetAllEpisodeStepsFromEpisode(episodeId);
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
            EpisodeFinished?.Invoke(null, EventArgs.Empty);
        }
        public static TimeSpan CalculateTime(DateTime startTime)
        {
            return DateTime.Now - startTime;
        }
        public static int CalculateScore(int maxScore, int mistakes)
        {
            return (int)((double)(maxScore-mistakes) / maxScore * 100);
        }
        public static int CalculateMaxScore(Episode episode)
        {
            return episode.EpisodeSteps.Sum(episodeStep => episodeStep.Word.Length);
        }

        public static double CalculateLetterPerMinute(TimeSpan time, int letters)
        {
            return Math.Round(letters / time.TotalMinutes);
        }
    }
}
