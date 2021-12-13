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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Cryptography;

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

            if(!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(username)) //Checking if user has entered all the information
            {
                if (new EmailAddressAttribute().IsValid(email)) //Checking if the given email has emailformat
                {
                    if (password.Equals(passwordcheck, StringComparison.Ordinal)) //Checking if the user has entered the password correctly
                    {
                        if (IsPasswordValid(password))
                        {
                            if (username.Length <= 20)
                            {
                                string salt = Argon2.CreateSalt();
                                string passHashed = Argon2.HashPassword(password, salt); //Hashing the password
                                bool Adduser = DBQueries.AddUser(username, email, passHashed, salt); //Adding new user to database
                                if (Adduser)
                                    NavigationController.NavigateToPage(Pages.LoginPage); //Returning to loginpage
                                else { error.Text = "Error: Service onbereikbaar / Bestaande gebruiker"; }
                            }
                            else { error.Text = "Error: Gebruikersnaam is te lang (max 20 tekens)"; }
                        }
                        else { error.Text = "Error: Wachtwoord bevat geen kleine of grote letter, nummer of minstens 8 tekens"; }
                    }
                    else { error.Text = "Error: Wachtwoorden komen niet overeen"; }
                }
                else { error.Text = "Error: Geen geldige E-mail"; }
            }
            else { error.Text = "Error: Lege velden"; }
        }

        private static bool IsPasswordValid(string password)
        {
            Regex containNumber = new(@"[0-9]+");
            Regex containUpperCase = new(@"[A-Z]+");
            Regex containLowerCase = new(@"[a-z]+");
            Regex containMinLength8Char = new(@".{8,}");

            return containNumber.IsMatch(password) && containUpperCase.IsMatch(password) && containLowerCase.IsMatch(password) && containMinLength8Char.IsMatch(password);
        }

    }
}
