using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class EpisodeResultPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Score { get => $"{EpisodeController.CurrentEpisodeResult?.Score ?? 0}%"; }
        public int Mistakes { get => EpisodeController.CurrentEpisodeResult?.Mistakes ?? 0; }
        //public TimeSpan Time { get => EpisodeController.CurrentEpisodeResult?.Time ?? TimeSpan.Zero; }
        public string Time { get => FormatTimespan(); }
        public EpisodeResultPageDataContext()
        {
            EpisodeController.EpisodeFinished += OnEpisodeFinished;
            //EpisodeController.WordChanged += OnWordChanged;
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