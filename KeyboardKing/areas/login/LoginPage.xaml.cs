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
            Console.WriteLine("test");
        }

        public override void OnTick()
        {
        }


        public void BLogin(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.ToString();
            string emailResult = DBQueries.GetEmail(email); // Get data from DB and compare data of email
            if (!emailResult.Equals(new string("F"), StringComparison.Ordinal))
            {
                if (email.Equals(emailResult, StringComparison.Ordinal))
                {
                    if (!DBQueries.GetPassword(boxPassword.Password.ToString()).Equals("F", StringComparison.Ordinal))
                    {
                        bool passwordResult = TripleDES.VerifyHash(email, boxPassword.Password.ToString());
                        if (passwordResult)
                        {
                            error.Content = "Login is succesful!";
                        }
                        else
                        {
                            error.Content = "Password is incorrect.";
                        }

                    }
                }
            }
            else
            {
                error.Content = "Email is incorrect";
            }
        }
    }
}
