using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Match
    {
        public int PlayerCount { get; set; }
        public string Host { get; set; }
        public int Id { get; set; }
        public string EpisodeName { get; set; }

        public static List<Match> ParseMatches(List<List<string>> input)
        {
            return input.Select(m => new Match()
            {
                PlayerCount = int.Parse(m[0]),
                Host = m[1],
                Id = int.Parse(m[2]),
                EpisodeName = m[3]
            }).ToList();
        }
    }
}
