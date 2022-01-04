using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ModelTest
{
    class Model_Episode
    {
        private Episode Episode { get; set; }

        [SetUp]
        public void Setup()
        {
            Episode = new Episode()
            {
                ChapterId = 1,
                ChapterName = "Beginner 1",
                ChapterEpisodeId = 1,
                Name = "F en J",
                Id = 1,
                Completed = true,
                HighScore = 13,
                EpisodeSteps = new Queue<EpisodeStep>(),
                PassThreshold = 80
            };
        }

        [Test]
        public void PropertiesAreAccessible()
        {
            bool r_a = Episode.ChapterId==1;
            bool r_b = Episode.ChapterName.Equals("Beginner 1");
            bool r_c = Episode.ChapterEpisodeId==1;
            bool r_d = Episode.Name.Equals("F en J");
            bool r_e = Episode.Id==1;
            bool r_f = Episode.Completed;
            bool r_g = Episode.HighScore==13;
            bool r_h = Episode.PassThreshold==80;

            Assert.IsTrue( ((r_a) && (r_b) && (r_c) && (r_d) && (r_e) && (r_f) && (r_g) && (r_h)) );
        }

        [Test]
        public void ParseEpisodes()
        {
            List<List<string>> data = new List<List<string>>()
            {
                new List<string>()
                {
                    "1",             // ChapterId
                    "Beginner 1",    // ChapterName
                    "1",             // ChapterEpisodeId
                    "F en J",        // Name
                    "1",             // Id
                    "false",         // Completed
                    ""               // HighScore
                },
                new List<string>()
                {
                    "2",             // ChapterId
                    "Beginner 2",    // ChapterName
                    "2",             // ChapterEpisodeId
                    "F en J",        // Name
                    "22",            // Id
                    "true",          // Completed
                    "130"            // HighScore
                }
            };

            List<Episode> result = Episode.ParseEpisodes(data);
            Assert.IsTrue( (result[0].Name.Equals("F en J")) && (result[1].Completed) );
        }
    }
}
