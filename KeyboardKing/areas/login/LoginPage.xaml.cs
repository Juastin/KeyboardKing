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

            // Save audio settings and get rid of the session if the user was logged in.
            User user = (User)Session.Get("student");
            if (user is not null) {
                if (user.AudioOn)
                {
                    DBQueries.UpdateAudioSetting(user.Id, 1);
                } else
                {
                    DBQueries.UpdateAudioSetting(user.Id, 0);
                }
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
                && !boxPassword.Password.Equals("", StringComparison.Ordinal) && boxPassword.Password != null)
            {
                User user = DBQueries.GetUserInfo(email);
                if (user != null)
                {
                    bool passwordResult = Argon2.VerifyHash(boxPassword.Password, user.Salt, user.Password);
                    if (passwordResult)
                    {
                        user.Password = user.Salt = null;
                        Session.Add("student", user);

                        // Set audio preference based on UserSettings
                        MusicPlayer.ShouldPlay = user.AudioOn;
                        AudioPlayer.ShouldPlay = user.AudioOn;
                        MusicPlayer.PlayNextFrom("menu_music");

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
