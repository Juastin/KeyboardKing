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

        private void Button_Click_Skill(object sender, RoutedEventArgs e) 
        {
            string[] result = (string[])Session.Get("student");
            DBQueries.AddSkill(((Button)sender).Tag.ToString(), result);
            NavigationController.NavigateToPage(Pages.ChaptersPage);
        }
    }
}
