using System;
using System.Collections.Generic;
using Model;
using NUnit.Framework;

namespace ModelTest
{
    class Model_MatchProgress
    {
        private MatchProgress Progress { get; set; }

        [SetUp]
        public void Setup()
        {
            Progress = new MatchProgress(13, "tester")
            {
                Progress = 100,
                Score = 10000,
                Mistakes = 0,
                LPM = 400,
                Time = TimeSpan.MaxValue
            };
        }

        [Test]
        public void PropertiesAreAccessible()
        {
            bool r_a = Progress.User.Id==13;
            bool r_b = Progress.User.Username.Equals("tester");
            bool r_c = Progress.Progress==100;
            bool r_d = Progress.Score==10000;
            bool r_e = Progress.Mistakes==0;
            bool r_f = Progress.LPM==400;
            bool r_g = Progress.Time==TimeSpan.MaxValue;

            Assert.IsTrue( ((r_a) && (r_b) && (r_c) && (r_d) && (r_e) && (r_f) && (r_g)) );
        }

        [Test]
        public void ParseMatchProgress()
        {
            List<List<string>> data = new List<List<string>>()
            {
                new List<string>()
                {
                    "2",          // User Id
                    "tester",     // User Username
                    "100",        // Progress
                    "10000",      // Score
                    "0",          // Mistakes
                    "400",        // LPM
                    "425670854"   // Time
                },
                new List<string>()
                {
                    "310",        // User Id
                    "benjamin",   // User Username
                    "99",         // Progress
                    "9010",       // Score
                    "2",          // Mistakes
                    "380",        // LPM
                    "425670854"   // Time
                }
            };

            List<MatchProgress> result = MatchProgress.ParseMatchProgress(data);

            Assert.IsTrue( (result[0].User.Username.Equals("tester") && result[0].Mistakes==0 && result[1].LPM==380) );
        }

        [Test]
        public void ParseSimpleProgress()
        {
            List<List<string>> data = new List<List<string>>()
            {
                new List<string>()
                {
                    "tester",     // User Username
                    "80",         // Progress
                },
                new List<string>()
                {
                    "benjamin",   // User Username
                    "95",         // Progress
                }
            };

            List<MatchProgress> result = MatchProgress.ParseSimpleProgress(data);

            Assert.IsTrue( (result[0].User.Username.Equals("tester") && result[0].Progress==80 && result[1].Progress==95) );
        }
    }
}
