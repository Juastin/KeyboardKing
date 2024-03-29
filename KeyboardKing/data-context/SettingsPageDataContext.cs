﻿using System;
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
        public string Dyslectic { get => $"{((User)Session.Get("student"))?.Dyslectic ?? false}" ; }
        public string AudioOn { get => $"{((User)Session.Get("student"))?.AudioOn ?? false}"; }

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
