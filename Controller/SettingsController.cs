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
        private static Dictionary<string, Font> _fonts;
        private static ResourceDictionary fontDictionary = Application.Current.Resources.MergedDictionaries[1];

        public static void Initialise()
        {
            _fonts = new Dictionary<string, Font>
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
        /// Changes font based on the given Font.
        /// </summary>
        /// <param name="font"></param>
        private static void ChangeFont(Font font)
        {
            fontDictionary.Clear();
            fontDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = font.FontUri });
        }
        /// <summary>
        /// Changes font based on the dyslectic bool of the user. If true the font Verdana will be showed, otherwise SegeoUI will be showed.
        /// </summary>
        /// <param name="dyslectic"></param>
        public static void ChangeDyslecticFont(bool dyslectic)
        {
            if (dyslectic)
            { 
                _fonts.TryGetValue("Verdana", out var font);
                ChangeFont(font);
            }
            else
            {
                _fonts.TryGetValue("SegoeUI", out var font);
                ChangeFont(font);
            }
        }

        public static bool DeleteAccount()
        {
            return DBQueries.DeleteUserAccount((User)Session.Get("student"));
        }
    }
}
