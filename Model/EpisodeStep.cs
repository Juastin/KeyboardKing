using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
