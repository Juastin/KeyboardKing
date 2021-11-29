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
        [SetUp]
        public void SetUp()
        {
            StartTime = DateTime.Now;
            Episode = new Episode();
            EpisodeStep e1 = new EpisodeStep();
            EpisodeStep e2 = new EpisodeStep();
            e1.Word = "kaas";
            e2.Word = "koekje";
            Episode.EpisodeSteps.Enqueue(e1);
            Episode.EpisodeSteps.Enqueue(e2);
        }

        [Test]
        public void CalculateTime_ReturnsCorrect()
        {
           TimeSpan diffTime = EpisodeController.CalculateTime(StartTime);
           TimeSpan tsStartTime = TimeSpan.FromTicks(StartTime.Ticks);
           Assert.AreNotEqual(tsStartTime, diffTime);
        }

        [Test]
        public void CalculateScore_ReturnCorrect()
        {
            int maxScore = EpisodeController.CalculateMaxScore(Episode);
            int score = EpisodeController.CalculateScore(maxScore, 5);
            Assert.AreEqual(50, score);
        }

        [Test]
        public void CalculateMaxScore_ReturnCorrect()
        {
            int maxScore = EpisodeController.CalculateMaxScore(Episode);
            Assert.AreEqual(10, maxScore);
        }
    }
}
