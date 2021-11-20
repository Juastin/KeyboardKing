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

        public static void Initialise(Episode episode)
        {
            _currentEpisode = episode;
            _currentEpisodeResult = new EpisodeResult();
            _wordIndex = 0;
            _wrongIndex = 0;
            NextEpisodeStep();
        }
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

        public static void CheckInput(char input)
        {
            NextLetter(_currentEpisodeStep.Word[_wordIndex].Equals(input));
        }
    }
}
