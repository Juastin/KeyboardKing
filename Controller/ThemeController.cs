using DatabaseController;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class ThemeController
    {
        public delegate void UserChange();
        public static event UserChange UserChanged;

        public static readonly Dictionary<string, Theme> DefaultThemes = new Dictionary<string, Theme>()
        {
             { "Light", new Theme("Light Theme", "resources/themes/LightTheme.xaml") },
             { "Dark", new Theme("Dark Theme", "resources/themes/DarkTheme.xaml") },
        };

        public static readonly Dictionary<string, Theme> Themes = new Dictionary<string, Theme>
            {
                { "Light", new Theme("Light Theme", "resources/themes/LightTheme.xaml") },
                { "Dark", new Theme("Dark Theme", "resources/themes/DarkTheme.xaml") },
                { "Space", new Theme("Space Theme", "resources/themes/SpaceTheme.xaml") },
                { "Chinese", new Theme("Chinese Theme", "resources/themes/ChineseTheme.xaml") },
                { "Paint", new Theme("Paint Theme", "resources/themes/PaintTheme.xaml") },
                { "Obsidian", new Theme("Obsidian Theme", "resources/themes/ObsidianTheme.xaml") },
                { "Hello beertje", new Theme("Hello beertje", "resources/themes/HelloBeertjeTheme.xaml") },
                { "Christmas", new Theme("Christmas Theme", "resources/themes/ChristmasTheme.xaml") },
            };

        public static Dictionary<string, Theme> UserThemes { get; private set; }

        public static string CurrentTheme;

        public static void Initialize()
        {
            ClearUserThemes();
            ShopController.ShopDataChanged += OnShopDataChanged;
        }

        /// <summary>
        /// Set all the themes the current user logged in owns.
        /// </summary>
        public static void SetUserThemes()
        {
            ClearUserThemes();
            List<Item> allItems = DBQueries.GetAllItems((User)Session.Get("student"));
            List<string> userItems = (from item in allItems
                                      where item.Type is ItemType.Theme
                                      && item.Purchased is "True"
                                      select item.Name).ToList();

            foreach (string item in userItems)
            {
                if (Themes.ContainsKey(item))
                {
                    KeyValuePair<string, Theme> theme = new KeyValuePair<string, Theme>(item, Themes[item]);
                    UserThemes.Add(theme.Key, theme.Value);
                }
            }
        }

        /// <summary>
        /// Clear the userthemes dictionary and create new one with the default themes.
        /// </summary>
        public static void ClearUserThemes()
        {
            UserThemes = new Dictionary<string, Theme>(DefaultThemes);
        }

        /// <summary>
        /// If for example a new theme is bought, reset the userthemes dictionary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnShopDataChanged(object sender, EventArgs e)
        {
            SetUserThemes();
        }

        /// <summary>
        /// Inboke the event to set the default theme of the user.
        /// </summary>
        public static void SetDefaultTheme()
        {
            UserChanged?.Invoke();
        }

        /// <summary>
        /// Methods that are called to prepare themedata of the current user.
        /// </summary>
        public static void SetUserThemeData()
        {
            ClearUserThemes();
            SetUserThemes();
            SetDefaultTheme();
        }

        /// <summary>
        /// Update the default theme of the user.
        /// </summary>
        public static void UpdateDefaultTheme()
        {
            DBQueries.UpdateDefaultTheme((User)Session.Get("student"), CurrentTheme);
        }
    }
}
