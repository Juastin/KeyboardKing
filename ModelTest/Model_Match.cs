using System.Collections.Generic;
using Model;
using NUnit.Framework;

namespace ModelTest
{
    class Model_Match
    {
        private Match Match { get; set; }

        [SetUp]
        public void Setup()
        {
            Match = new Match(
                231,
                new User(){Username = "player_host"},
                new Episode(){Id = 1, Name = "F en J"},
                MatchState.Open
            );
        }

        [Test]
        public void PropertiesAreAccessible()
        {
            bool r_a = Match.Id==231;
            bool r_b = Match.Host.Username.Equals("player_host");
            bool r_c = Match.Episode.Id==1;
            bool r_d = Match.State==MatchState.Open;

            Assert.IsTrue( (r_a) && (r_b) && (r_c) && (r_d) );
        }

        [Test]
        public void ParseMatch()
        {
            List<List<string>> data = new List<List<string>>()
            {
                new List<string>()
                {
                    "122",       // Match Id
                    "1",         // Match State
                    "12",        // Host Id
                    "tester",    // Host Username
                    "7",         // Episode Id
                    "F en J"     // Episode Name
                }
            };
            Match result = Match.ParseMatch(data);

            Assert.IsTrue( (result.Id==122) && (result.Host.Username.Equals("tester")) && (result.Episode.Name.Equals("F en J")) );
        }

        [Test]
        public void ParseMatches()
        {
            List<List<string>> data = new List<List<string>>()
            {
                new List<string>()
                {
                    "122",               // PlayerCount
                    "12",                // Host Id
                    "tester",            // Host Username
                    "23",                // Match Id
                    "0",                 // Match State
                    "7",                 // Episode Id
                    "F en J"             // Episode Name
                },
                new List<string>()
                {
                    "999",               // PlayerCount
                    "67",                // Host Id
                    "benjamin",          // Host Username
                    "55",                // Match Id
                    "2",                 // Match State
                    "19",                // Episode Id
                    "G en K"             // Episode Name
                },
            };
            List<Match> result = Match.ParseMatches(data);

            Assert.IsTrue( (result[0].PlayerCount==122) && (result[0].Host.Username.Equals("tester")) && (result[0].Episode.Name.Equals("F en J")) && (result[1].State==MatchState.Finished) );
        }
    }
}
