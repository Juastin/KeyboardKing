using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace KeyboardKing.data_context
{
    public class SettingsPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public User Student { get => SettingsController.Student; }
        public string Dyslectic { get => $"{SettingsController.Student?.Dyslectic ?? false}" ; }

        public SettingsPageDataContext()
        {
            SettingsController.Refresh += OnRefresh;
        }
        private void OnRefresh(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
