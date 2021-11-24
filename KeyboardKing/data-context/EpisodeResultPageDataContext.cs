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

        public int Score
        {
            get => EpisodeController.CurrentEpisodeResult?.Score ?? 0;
        }

        public EpisodeResultPageDataContext()
        {
            EpisodeController.EpisodeFinished += OnEpisodeFinished;
            //EpisodeController.WordChanged += OnWordChanged;
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}