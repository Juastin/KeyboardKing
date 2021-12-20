using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace KeyboardKing.data_context
{
    public class EpisodeResultPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Score { get => $"{EpisodeController.CurrentEpisodeResult?.Score ?? 0}p"; }
        public int Mistakes { get => EpisodeController.CurrentEpisodeResult?.Mistakes ?? 0; }
        public string Accuracy { get => $"{EpisodeController.CurrentEpisodeResult?.Accuracy ?? 0}%"; }
        public string Time { get => FormatTimespan(); }
        public double LettersPerMinute { get => EpisodeController.CurrentEpisodeResult?.LettersPerMinute ?? 0; }
        public EpisodeResultPageDataContext()
        {
            EpisodeController.EpisodeResultUpdated += OnEpisodeFinished;
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
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