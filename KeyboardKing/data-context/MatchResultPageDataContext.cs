using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace KeyboardKing.data_context
{
    public class MatchResultPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Score { get => $"{EpisodeController.CurrentEpisodeResult?.Score ?? 0}p"; }
        public int Mistakes { get => EpisodeController.CurrentEpisodeResult?.Mistakes ?? 0; }
        public string Accuracy { get => $"{EpisodeController.CurrentEpisodeResult?.Accuracy ?? 0}%"; }
        public string Time { get => FormatTimespan(); }
        public double LettersPerMinute { get => EpisodeController.CurrentEpisodeResult?.LettersPerMinute ?? 0; }

        public string Winner1 { get => MatchController.Winner1; }
        public string Winner2 { get => MatchController.Winner2; }
        public string Winner3 { get => MatchController.Winner3; }

        public string Score1 { get => MatchController.Score1 == 0 ? "" : MatchController.Score1.ToString(); }
        public string Score2 { get => MatchController.Score2 == 0 ? "" : MatchController.Score2.ToString(); }
        public string Score3 { get => MatchController.Score3 == 0 ? "" : MatchController.Score3.ToString(); }
        public MatchResultPageDataContext()
        {
            EpisodeController.EpisodeFinished += OnRefresh;
            MatchController.Refresh += OnRefresh;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        private string FormatTimespan()
        {
            TimeSpan span = EpisodeController.CurrentEpisodeResult?.Time ?? TimeSpan.Zero;
            return string.Format("{0}min {1}sec",
                (int)span.TotalMinutes,
                span.Seconds);
        }
    }
}