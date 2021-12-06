using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EpisodeResult
    {
        public int Mistakes { get; set; }
        private int _score;
        public int Score { 
            get 
            { 
                return _score; 
            } 
            set 
            {
                _score = value;
            } 
        }
        public int MaxScore { get; set; }
        public TimeSpan Time { get; set; }
        public double LettersPerMinute { get; set; }
    }
}
