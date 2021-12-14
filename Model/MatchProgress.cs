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
        public MatchState MatchState { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int EpisodeId { get; set; }
        public string EpisodeName { get; set; }
        public int Progress { get; set; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public int LPM { get; set; }
        public TimeSpan Time { get; set; }

        public static List<MatchProgress> ParseMatchProgress(List<List<string>> input)
        {
            return input.Select(p => new MatchProgress()
            {
                MatchId = int.Parse(p[0]),
                Username = p[1],
                EpisodeName = p[2],
                Progress = int.Parse(p[3]),
                Score = int.Parse(p[4]),
                Mistakes = int.Parse(p[5]),
                LPM = int.Parse(p[6]),
                Time = TimeSpan.FromTicks(long.Parse(p[7])),
                HostId = int.Parse(p[8]),
                EpisodeId = int.Parse(p[9]),
                MatchState = (MatchState)Enum.Parse(typeof(MatchState), p[10]),
                UserId = int.Parse(p[11])
            }).ToList();
        }

        /// <summary>
        /// Only returns a list of <see cref="MatchProgress"/> with <see cref="MatchProgress.Username"/> and <see cref="MatchProgress.Progress"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<MatchProgress> ParseSimpleProgress(List<List<string>> input)
        {
            return input.Select(p => new MatchProgress() { Username = p[0], Progress = int.Parse(p[1]) }).ToList();
        }
    }
}
