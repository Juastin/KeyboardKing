using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DatabaseController;
using Model;

namespace Controller
{
    public static class SettingsController
    {
        public static event EventHandler Refresh;
        private static ResourceDictionary fontDictionary = Application.Current.Resources.MergedDictionaries[1];
        public static void Initialise()
        {
            Refresh?.Invoke(null,EventArgs.Empty);
        }
    }
}
