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
        private static EpisodeStep _currentEpisodeStep;
        private static EpisodeResult _currentEpisodeResult;
        private static int _wordIndex;
        private static int _wrongIndex;

        public static event EventHandler WordChanged;

        public static string Word { get => _currentEpisodeStep?.Word; }
        public static string WordOverlayCorrect { get =>_currentEpisodeStep?.Word.Substring(0, _wordIndex); }
        public static string WordOverlayWrong { get =>_currentEpisodeStep?.Word.Substring(0, _wrongIndex); }

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
            _currentEpisodeResult = new EpisodeResult();
            _wordIndex = 0;
            _wrongIndex = 0;
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
                _currentEpisodeStep = step;
            else
                throw new Exception("TryDequeue went wrong");

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
                _currentEpisodeResult.Mistakes++;
            }
                
            if (_wordIndex >= _currentEpisodeStep.Word.Length)
                NextEpisodeStep();
            else
                WordChanged?.Invoke(null, new EventArgs());
        }

        /// <summary>
        /// Checks user input and parses the result to <see cref="NextLetter(bool)"/>
        /// </summary>
        /// <param name="input">Input from the user</param>
        public static void CheckInput(char input)
        {
            NextLetter(_currentEpisodeStep.Word[_wordIndex].Equals(input));
        }
    }
}
