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
            if (episode.EpisodeSteps.TryDequeue(out EpisodeStep step))
                _currentEpisodeStep = step;
            else
                throw new Exception("TryDequeue went wrong");
        }
    }
}
