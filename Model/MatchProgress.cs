using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MatchProgress
    {
        public User User { get; set; }
        public int Progress { get; set; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public int LPM { get; set; }
        public TimeSpan Time { get; set; }

        public MatchProgress(int? userId, string username)
        {
            User = new User() { Id = userId ?? -1, Username = username };
        }

        public static List<MatchProgress> ParseMatchProgress(List<List<string>> input)
        {
            return input.Select(p => new MatchProgress(int.Parse(p[11]), p[1])
            {
                Progress = int.Parse(p[3]),
                Score = int.Parse(p[4]),
                Mistakes = int.Parse(p[5]),
                LPM = int.Parse(p[6]),
                Time = TimeSpan.FromTicks(long.Parse(p[7])),
            }).ToList();
        }

        /// <summary>
        /// Only returns a list of <see cref="MatchProgress"/> with <see cref="User.Username"/> and <see cref="MatchProgress.Progress"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<MatchProgress> ParseSimpleProgress(List<List<string>> input)
        {
            return input.Select(p => new MatchProgress(null, p[0]) { Progress = int.Parse(p[1]) }).ToList();
        }
    }
}
