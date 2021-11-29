﻿using KeyboardKing.core;
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
        }

        public override void OnShadow()
        {
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
                List<List<string>> results = DBQueries.GetUserInfo(email);
                if (results.Any())
                {
                    bool passwordResult = Encryption.VerifyHash(boxPassword.Password, results[0][4], results[0][3]);
                    if (passwordResult)
                    {
                        ClearText();
                        string[] Items = {results[0][0], results[0][1], results[0][2], results[0][5]};
                        Session.Add("student", Items);

                        if (results[0][5] == string.Empty)
                        {
                            Navigate("RegisterSkillPage");
                        }
                        else
                        {
                            Navigate("ChaptersPage");
                        }
                    }
                    else { message += "Wachtwoord is incorrect"; }
                }
                else { message += "Email is incorrect"; }
            }
            else { message += "Email of wachtwoord is niet ingevuld"; }
            error.Text = message;
        }

        public void BToRegistration(object sender, RoutedEventArgs e)
        {
            ClearText();
            ButtonNavigate(sender, e);
        }

        private void ClearText()
        {
            txtEmail.Clear();
            error.Text = "";
        }
    }
}
