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

        public static string Word { get => _currentEpisodeStep.Word; }
    }
}
