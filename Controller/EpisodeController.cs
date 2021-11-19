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
        public static int _wordIndex;

        public static event EventHandler WordChanged;

        public static string Word { get => _currentEpisodeStep?.Word; }
        public static string WordOverlay { get =>_currentEpisodeStep?.Word.Substring(0, _wordIndex); }

        public static void Initialise(Episode episode)
        {
            _currentEpisode = episode;
            _wordIndex = 0;
            NextEpisodeStep();
        }
        private static void NextEpisodeStep()
        {
            _wordIndex = 0;
            if(_currentEpisode.EpisodeSteps.TryDequeue(out EpisodeStep step))
                _currentEpisodeStep = step;
            else
                throw new Exception("TryDequeue went wrong");

            WordChanged?.Invoke(null, new EventArgs());
        }

        private static void NextLetter()
        {
            _wordIndex++;
            if (_wordIndex >= _currentEpisodeStep.Word.Length)
                NextEpisodeStep();
            else
                WordChanged?.Invoke(null, new EventArgs());
        }

        public static bool CheckInput(char input)
        {
            if (_currentEpisodeStep.Word[_wordIndex].Equals(input))
                NextLetter();
            else
                return false;

            return true;
        }
    }
}
