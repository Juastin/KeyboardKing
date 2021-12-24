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
        private static ResourceDictionary themeDictionary = Application.Current.Resources.MergedDictionaries[0];
        private static ResourceDictionary fontDictionary = Application.Current.Resources.MergedDictionaries[1];

        public SettingsPage(MainWindow w) : base(w)
        {
            InitializeComponent();
            
            ThemeController.Initialize();
            ThemeController.UserChanged += OnUserChanged;
        }

        /// <summary>
        /// Changes application theme when given theme param is found in the dictionary
        /// </summary>
        /// <param name="theme">All themes are defined in the theme dictionary</param>
        private static void ChangeTheme(string themeName)
        {
            if (themeName != null && ThemeController.Themes.TryGetValue(themeName, out var theme))
            {
                themeDictionary.Clear();
                themeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = theme.ThemeUri });
                NavigationController.ChangeTheme();

                ThemeController.CurrentTheme = themeName;
            }
        }
        /// <summary>
        /// Changes font based on the given Font.
        /// </summary>
        /// <param name="font"></param>
        public static void ChangeFont(Font font)
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
                SettingsController.Fonts.TryGetValue("Verdana", out var font);
                ChangeFont(font);
            }
            else
            {
                SettingsController.Fonts.TryGetValue("SegoeUI", out var font);
                ChangeFont(font);
            }
        }
        private void CBTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeTheme((string)CBTheme.SelectedValue);
        }

        public override void OnLoad()
        {
            SettingsController.RefreshWpf();
            User user = (User)Session.Get("student");
            AudioCheckBox.IsChecked = !user.AudioOn;

            UpdateComboBox();
        }
        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void OnAudioCheckClick(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
            {
                User user = (User)Session.Get("student");
                if ((bool)box.IsChecked)
                {
                    user.AudioOn = false;
                    MusicPlayer.ShouldPlay = false;
                    AudioPlayer.ShouldPlay = false;
                } else
                {
                    user.AudioOn = true;
                    MusicPlayer.ShouldPlay = true;
                    AudioPlayer.ShouldPlay = true;
                }
                Session.Add("student", user);
            }
        }

        private void CheckedDyslectic(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
            {
                User user = (User)Session.Get("student");
                user.Dyslectic = (bool)box.IsChecked;
                ChangeDyslecticFont(user.Dyslectic);
                Session.Add("student", user);
            }
        }

        public void UpdateComboBox()
        {
            CBTheme.ItemsSource = ThemeController.UserThemes;
            CBTheme.DisplayMemberPath = "Value.ThemeTitle";
            CBTheme.SelectedValuePath = "Key";
        }

        public void SetDefaultTheme(string theme)
        {
            CBTheme.SelectedValue = theme;
        }

        public void OnUserChanged()
        {
            UpdateComboBox();

            //Set theme to the default theme the user has.
            SetDefaultTheme(((User)Session.Get("student")).Theme);
        }

        private void DeleteAccount(object sender, RoutedEventArgs e)
        {
            MessageController.ShowConfirmation(Pages.ConfirmationPage, "Weet je het zeker?", Pages.SettingsPage, Pages.SettingsPage);
        }

        /// <summary>
        /// Checks if user has confirmed to delete account and delete the account
        /// </summary>
        private static void CheckDeleteAccount()
        {
            if (Session.Get("deleteUser") != null && (bool)Session.Get("deleteUser"))
            {
                if (SettingsController.DeleteAccount())
                {
                    MessageController.Show(Pages.MessagePage, "Je account is verwijderd", Pages.LoginPage, -1);
                }
                else
                {
                    MessageController.Show(Pages.MessagePage, "Je account kan op dit moment niet worden verwijderd", Pages.SettingsPage, -1);
                }
            }
        }
    }
}
