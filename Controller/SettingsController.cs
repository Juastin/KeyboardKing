using System;
using System.Collections.Generic;
using System.Windows;
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
            Refresh?.Invoke(null,EventArgs.Empty);

            _fonts = new Dictionary<string, Font>
            {
                { "SegoeUI", new Font("SegoeUI", "resources/fonts/SegoeUI.xaml") },
                { "Verdana", new Font("Verdana", "resources/fonts/Verdana.xaml") },
            };
        }
        private static void ChangeFont(Font font)
        {
            fontDictionary.Clear();
            fontDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = font.FontUri });
        }

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
    }
}
