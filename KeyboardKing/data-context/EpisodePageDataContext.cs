using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class EpisodePageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Word { get => EpisodeController.Word; }

        public EpisodePageDataContext()
        {
            EpisodeController.WordChanged += OnWordChanged;
        }

        private void OnWordChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
