using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class EpisodeController
    {
        private static Episode _currentEpisode;
        private static EpisodeStep _currentEpisodeStep;
        private static int _wordIndex;

        public static string Word { get => _currentEpisodeStep?.Word; }

        public static void Initialise(Episode episode)
        {
            _currentEpisode = episode;
            _wordIndex = 0;
            NextEpisodeStep();
        }
        public static void NextEpisodeStep()
        {
            if(_currentEpisode.EpisodeSteps.TryDequeue(out EpisodeStep step))
                _currentEpisodeStep = step;
            else
                throw new Exception("TryDequeue went wrong");
        }
        public static bool CheckInput(char input)
        {

            if (_currentEpisodeStep.Word[_wordIndex].Equals(input))
                _wordIndex++;
            else
                return false;

            return true;
        }
    }
}
