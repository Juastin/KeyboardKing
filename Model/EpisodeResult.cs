using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EpisodeResult
    {
        public int Mistakes { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public TimeSpan Time { get; set; }
    }
}
