using KeyboardKing.core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Controller;
using Model;
using System.Linq;
using DatabaseController;

namespace KeyboardKing.areas.main
{

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : JumpPage
    {
        private static readonly Dictionary<string, Theme> _defaultThemes = new Dictionary<string, Theme>()
        {
             { "Light", new Theme("Light Theme", "resources/themes/LightTheme.xaml") },
             { "Dark", new Theme("Dark Theme", "resources/themes/DarkTheme.xaml") },
        };

        private static Dictionary<string, Theme> _themes;
        private static Dictionary<string, Theme> _userThemes;
        private ResourceDictionary themeDictionary = Application.Current.Resources.MergedDictionaries[0];

        public SettingsPage(MainWindow w) : base(w)
        {
            InitializeComponent();

            _themes = new Dictionary<string, Theme>
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

            ClearUserThemes();
            UpdateComboBox();
        }
        /// <summary>
        /// Changes application theme when given theme param is found in the dictionary
        /// </summary>
        /// <param name="theme">All themes are defined in the theme dictionary</param>
        private void ChangeTheme(Theme theme)
        {
            themeDictionary.Clear();
            themeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = theme.ThemeUri });
            NavigationController.ChangeTheme();
        }
        private void CBTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object value = CBTheme.SelectedValue;
            if (value != null && _themes.TryGetValue((string)value, out var theme))
            {
                ChangeTheme(theme);
            }
        }
        public override void OnLoad()
        {
            SetUserThemes();
            UpdateComboBox();

            /* Set theme to theme the student has as his default.
            SetTheme(((User)Session.Get("student")).Theme);
            */
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

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
                if (_themes.ContainsKey(item))
                {
                    KeyValuePair<string, Theme> theme = new KeyValuePair<string, Theme>(item, _themes[item]);
                    _userThemes.Add(theme.Key, theme.Value);
                }
            }
        }

        public static void ClearUserThemes()
        {
            _userThemes = new Dictionary<string, Theme>(_defaultThemes);
        }

        public void UpdateComboBox()
        {
            CBTheme.ItemsSource = _userThemes;
            CBTheme.DisplayMemberPath = "Value.ThemeTitle";
            CBTheme.SelectedValuePath = "Key";
        }

        public void SetTheme(string theme)
        {
            CBTheme.SelectedValue = theme;
        }
    }
}
