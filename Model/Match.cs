using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum MatchState
    {
        Open, Started, Finished
    }

    public class Match
    {
        public int Id { get; set; }
        public User Host { get; set; }
        public MatchState State { get; set; }
        public Episode Episode { get; set; }
        public int PlayerCount { get; set; }

        public static List<Match> ParseMatches(List<List<string>> input)
        {
            return input.Select(m => new Match()
            {
                PlayerCount = int.Parse(m[0]),
                Host = new User() { Id = int.Parse(m[1]), Username = m[2] },
                Id = int.Parse(m[3]),
                State = (MatchState)Enum.Parse(typeof(MatchState), m[4]),
                Episode = new Episode() { Id = int.Parse(m[5]), Name = m[6] },
            }).ToList();
        }
    }
}
