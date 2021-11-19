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
            if (password.Equals(passwordcheck))
            {
                bool Adduser = DBQueries.AddUser(email, username, password);
                if (Adduser)
                {
                    Navigate("LoginPage");
                }
            }
        }
    }
}
