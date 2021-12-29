using System;

namespace Model
{
    public class EpisodeResult
    {
        public int Mistakes { get; set; }
        public int Score { get; set; }
        private int _accuracy;
        public int Accuracy
        {
            get => _accuracy;
            set 
            { 
                if (value >= 0 || value <= 100)
                    _accuracy = value;
                if (value < 0)
                    _accuracy = 0;
                if (value > 100)
                    _accuracy = 100;
            } 
        }
        public int MaxScore { get; set; }
        public TimeSpan Time { get; set; }
        public double LettersPerMinute { get; set; }

        public bool Passed { get; set; }
    }
}
