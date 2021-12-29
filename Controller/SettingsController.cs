using System;
using System.Collections.Generic;
using System.Windows;
using DatabaseController;
using Model;

namespace Controller
{
    public static class SettingsController
    {
        public static event EventHandler Refresh;
        public static Dictionary<string, Font> Fonts;

        public static void Initialise()
        {
            Fonts = new Dictionary<string, Font>
            {
                { "SegoeUI", new Font("SegoeUI", "resources/fonts/SegoeUI.xaml") },
                { "Verdana", new Font("Verdana", "resources/fonts/Verdana.xaml") },
            };
        }

        /// <summary>
        /// Refresh event that fires to refresh wpf checkboxes to show actual values.
        /// Subscription is found in SettingsPageDataContext.
        /// </summary>
        public static void RefreshWpf()
        {
            Refresh?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// User account that will be deleted
        /// </summary>
        /// <returns></returns>
        public static bool DeleteAccount()
        {
            return DBQueries.DeleteUserAccount((User)Session.Get("student"));
        }
    }
}
