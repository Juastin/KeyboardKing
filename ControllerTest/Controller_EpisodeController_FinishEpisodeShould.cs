using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Controller;

namespace ControllerTest
{
    public class Controller_EpisodeController_FinishEpisodeShould
    {
        public DateTime startTime;
        [SetUp]
        public void SetUp()
        {
            startTime = DateTime.Now;
        }

        [Test]
        public void CalculateTime_ReturnsCorrect()
        {
           TimeSpan diffTime = EpisodeController.CalculateTime(startTime);
           TimeSpan tsStartTime = TimeSpan.FromTicks(startTime.Ticks);
           Assert.AreNotEqual(tsStartTime, diffTime);
        }
    }
}
