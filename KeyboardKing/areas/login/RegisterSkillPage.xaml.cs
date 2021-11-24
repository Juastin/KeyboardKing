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
    /// Interaction logic for RegistrationSkillPage.xaml
    /// </summary>
    public partial class RegisterSkillPage : JumpPage
    {
        public RegisterSkillPage(MainWindow w) : base(w)
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

        private void Button_Click_Beginner(object sender, RoutedEventArgs e)
        {
            DBQueries.AddSkill("beginner");
        }

        private void Button_Click_Gemiddeld(object sender, RoutedEventArgs e)
        {
            DBQueries.AddSkill("gemiddeld");
        }

        private void Button_Click_Gevorderd(object sender, RoutedEventArgs e)
        {
            DBQueries.AddSkill("gevorderd");
        }
    }
}
