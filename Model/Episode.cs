using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Episode
    {
        public Queue<EpisodeStep> EpisodeSteps { get; set; }
        public int PassThreshold { get; set; }

        public Episode()
        {
            EpisodeSteps = new Queue<EpisodeStep>();
        }
    }
}
