using KeyboardKing.core;
using System;
using System.Windows;
using Controller;
using Model;
using Cryptography;
using DatabaseController;

namespace KeyboardKing.areas.login
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : JumpPage
    {
        public RegisterPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
        }

        public override void OnShadow()
        {
            txtemail.Clear();
            txtusername.Clear();
            error.Text = "";
        }

        public override void OnTick()
        {
        }

        public void BRegister(object sender, RoutedEventArgs e)
        {
            string email = txtemail.Text.ToString();
            string username = txtusername.Text.ToString();

            string password = password1.Password;
            string passwordcheck = password2.Password;
            string message = "Error: ";

            if(!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(username)) //Checks if user has entered all the information
            {
                if (AuthenticationController.IsEmail(email))
                {
                    if (password.Equals(passwordcheck, StringComparison.Ordinal)) // Checks if user entered password correct
                    {
                        if (AuthenticationController.IsPasswordValid(password))
                        {
                            if (username.Length <= 20)
                            {
                                string[] passwordHashed = AuthenticationController.HashPassword(password);
                                if (AuthenticationController.AddUser(username, email, passwordHashed[0], passwordHashed[1]))
                                    NavigationController.NavigateToPage(Pages.LoginPage);
                                else {  message += "Service onbereikbaar / Bestaande gebruiker"; }
                            }
                            else { message += "Gebruikersnaam is te lang (max 20 tekens)"; }
                        }
                        else { message += "Wachtwoord bevat geen kleine of grote letter, nummer of minstens 8 tekens"; }
                    }
                    else { message += "Wachtwoorden komen niet overeen"; }
                }
                else { message += "e-mail is niet geldig"; }
            }
            else { message += "Lege velden"; }
            error.Text = message;
        }

    }
}
