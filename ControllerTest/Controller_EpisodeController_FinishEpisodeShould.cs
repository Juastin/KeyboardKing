using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Controller;
using Model;

namespace ControllerTest
{
    public class Controller_EpisodeController_FinishEpisodeShould
    {
        public DateTime StartTime { get; set; }
        public Episode Episode { get; set; }
        public TimeSpan TwoMinute { get; set; }
        [SetUp]
        public void SetUp()
        {
            StartTime = DateTime.Now;
            TwoMinute = TimeSpan.FromMinutes(2);
            Episode = new Episode();
            EpisodeStep e1 = new EpisodeStep();
            EpisodeStep e2 = new EpisodeStep();
            
            e1.Word = "kaas";
            e2.Word = "koekje";
            Episode.EpisodeSteps.Enqueue(e1);
            Episode.EpisodeSteps.Enqueue(e2);

            //EpisodeController.CurrentEpisodeResult.MaxScore = EpisodeController.CalculateTotalLetters(Episode);

            //User User = new User();
            //Session.Add("student",User);
            //Session.Add("episodeId", "3");

            EpisodeController.Initialise(Episode, false);
        }

        //[Test]
        //public void FinishEpisode_FillsEpisodeResult()
        //{
        //    var emptyEpisodeResults = EpisodeController.CurrentEpisodeResult;
        //    EpisodeController.FinishEpisode();
        //    var filledEpisodeResults = EpisodeController.CurrentEpisodeResult;
        //    Assert.AreNotEqual(emptyEpisodeResults,filledEpisodeResults);
        //
        //}

        [Test]
        public void CalculateMaxScore_ReturnCorrect()
        {
            Assert.AreEqual(10, EpisodeController.CurrentEpisodeResult.MaxScore);
        }

        [Test]
        public void CalculateLPM_ReturnCorrect()
        { 
            double lpm = EpisodeController.CalculateLetterPerMinute(TwoMinute, EpisodeController.CurrentEpisodeResult.MaxScore);
            Assert.AreEqual(5.0, lpm);
        }
    }
}
