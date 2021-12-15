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

        public Match(int id, User host, Episode episode, MatchState state)
        {
            Id = id;
            Host = host;
            Episode = episode;
            State = state;
        }

        public static Match ParseMatch(List<List<string>> input)
        {
            Episode episode = new Episode() { Id = int.Parse(input[0][4]), Name = input[0][5] };
            User host = new User() { Id = int.Parse(input[0][2]), Username = input[0][3] };
            MatchState state = (MatchState)Enum.Parse(typeof(MatchState), input[0][1]);

            return new Match(int.Parse(input[0][0]), host, episode, state);
        }

        public static List<Match> ParseMatches(List<List<string>> input)
        {

            return input.Select(m => {
                Episode episode = new Episode() { Id = int.Parse(m[5]), Name = m[6] };
                User host = new User() { Id = int.Parse(m[1]), Username = m[2] };
                MatchState state = (MatchState)Enum.Parse(typeof(MatchState), m[4]);

                return new Match(int.Parse(m[3]), host, episode, state)
                {
                    PlayerCount = int.Parse(m[0]),
                    State = (MatchState)Enum.Parse(typeof(MatchState), m[4]),
                };
            }).ToList();
        }
    }
}
