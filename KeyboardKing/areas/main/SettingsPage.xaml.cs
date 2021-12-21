using KeyboardKing.core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Controller;
using Model;
using DatabaseController;

namespace KeyboardKing.areas.main
{

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : JumpPage
    {
        private Dictionary<string, Theme> _themes;
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

            CBTheme.ItemsSource = _themes;
            CBTheme.SelectedValue = "Light";
            CBTheme.DisplayMemberPath = "Value.ThemeTitle";
            CBTheme.SelectedValuePath = "Key";
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
            if (_themes.TryGetValue((string)value, out var theme))
            {
                ChangeTheme(theme);
            }
        }

        public override void OnLoad()
        {
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void DeleteAccount(object sender, RoutedEventArgs e)
        {
            //MessageController.ShowConfirmation(Pages.ConfirmationPage, "Weet je het zeker?", Pages.SettingsPage, Pages.LoginPage);
            if (DBQueries.DeleteUserAccount((User)Session.Get("student")))
            {
                MessageController.Show(Pages.MessagePage, "Je account is verwijderd", Pages.LoginPage, -1);
            }
        }
    }
}
