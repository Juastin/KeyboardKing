using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MatchProgress
    {
        public int HostId { get; set; }
        public int MatchId { get; set; }
        public string Username { get; set; }
        public string EpisodeName { get; set; }
        public int Progress { get; set; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public int LPM { get; set; }
        public TimeSpan Time { get; set; }

        public static List<MatchProgress> ParseOpponentProgress(List<List<string>> input)
        {
            return input.Select(m => new MatchProgress() { Username = m[0], Progress = int.Parse(m[1]) }).ToList();
        }
    }
}
