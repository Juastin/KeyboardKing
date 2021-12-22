using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class EpisodeStep
    {
        public string Word { get; set; }

        public static List<EpisodeStep> ParseEpisodeSteps(List<List<string>> input)
        {
            List<EpisodeStep> episodeSteps = input.Select(e => new EpisodeStep() {
                Word = e[0] 
            }).ToList();

            return episodeSteps;
        }
    }
}
