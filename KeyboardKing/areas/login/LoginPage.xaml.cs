using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller;
using Model;

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
            if (Session.Get("student") is not null) {
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
                    bool passwordResult = Encryption.VerifyHash(boxPassword.Password, user.Salt, user.Password);
                    if (passwordResult)
                    {
                        user.Password = user.Salt = null;
                        Session.Add("student", user);

                        if (user.SkillLevel == null)
                        {
                            NavigationController.NavigateToPage(Pages.RegisterSkillPage);
                            return;
                        }
                        else
                        {
                            MusicPlayer.PlayNextFrom("menu_music");
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
