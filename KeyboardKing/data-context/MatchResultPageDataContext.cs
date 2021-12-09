using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class MatchResultPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Score { get => $"{MatchController.CurrentEpisodeResult?.Score ?? 0}%"; }
        public int Mistakes { get => MatchController.CurrentEpisodeResult?.Mistakes ?? 0; }
        public string Time { get => FormatTimespan(); }
        public double LettersPerMinute { get => MatchController.CurrentEpisodeResult?.LettersPerMinute ?? 0; }
        public MatchResultPageDataContext()
        {
            MatchController.EpisodeFinished += OnEpisodeFinished;
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        private string FormatTimespan()
        {
            TimeSpan span = MatchController.CurrentEpisodeResult?.Time ?? TimeSpan.Zero;
            return string.Format("{0}min {1}sec",
                (int)span.TotalMinutes,
                span.Seconds);
        }
    }
}