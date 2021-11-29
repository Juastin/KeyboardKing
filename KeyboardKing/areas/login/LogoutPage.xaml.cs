using Controller;
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

namespace KeyboardKing.areas.login
{
    /// <summary>
    /// Interaction logic for LogoutPage.xaml
    /// </summary>
    public partial class LogoutPage : JumpPage
    {
        public LogoutPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            Session.Flush();
            Navigate("LoginPage");
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }
    }
}
