using KeyboardKing.core;
using System;
using System.Windows;
using System.Windows.Input;
using Controller;
using Model;
using Cryptography;
using DatabaseController;

namespace KeyboardKing.areas.login
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : JumpPage
    {
        public LoginPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            MusicPlayer.Stop();

            User user = (User)Session.Get("student");
            if (user is not null) 
            {
                // Save audio settings if they were changed.
                if (user.AudioOn != user.AudioOnAtLogin)
                {
                    DBQueries.UpdateAudioSetting(user.Id, user.AudioOn);
                }
                if (user.Dyslectic != user.DyslecticAtLogin)
                {
                    DBQueries.UpdateDyslecticSettings(user.Id, user.Dyslectic);
                }
                if (user.Theme != ThemeController.CurrentTheme) {
                    ThemeController.UpdateDefaultTheme();
                }
                // Flush the session if the user was logged in when entering the login page.
                Session.Flush();
            }
        }

        public override void OnShadow()
        {
            txtEmail.Clear();
            error.Text = "";
        }

        public override void OnTick()
        {
        }

        public void BLogin(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.ToString();
            string message = "Error: ";

            if (!email.Equals("", StringComparison.Ordinal) && email != null
                && !boxPassword.Password.Equals("", StringComparison.Ordinal) && boxPassword.Password != null) // Checks if fields isn't empty
            {
                User user = AuthenticationController.GetUserInfo(email);
                if (user != null)
                {
                    if (AuthenticationController.VerifyPassword(user, boxPassword.Password))
                    {
                        user.Password = user.Salt = null;
                        user.AudioOnAtLogin = user.AudioOn;
                        user.DyslecticAtLogin = user.Dyslectic;

                        Session.Add("student", user);

                        // Set audio preference based on UserSettings
                        MusicPlayer.ShouldPlay = user.AudioOn;
                        AudioPlayer.ShouldPlay = user.AudioOn;
                        MusicPlayer.PlayNextFrom("menu_music");

                        // Set default Theme based on UserSettings
                        ThemeController.SetUserThemeData();

                        if (user.SkillLevel == SkillLevel.none)
                        {
                            NavigationController.NavigateToPage(Pages.RegisterSkillPage);
                            return;
                        }
                        else
                        {
                            NavigationController.NavigateToPage(Pages.ChaptersPage);
                            return;
                        }
                    }
                    else { message += "Wachtwoord is incorrect"; }
                }
                else { message += "Email is incorrect"; }
            }
            else { message += "Email of wachtwoord is niet ingevuld"; }
            error.Text = message;
        }

        private void OnKeyDownLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { BLogin(null, new RoutedEventArgs()); }
        }
    }
}
