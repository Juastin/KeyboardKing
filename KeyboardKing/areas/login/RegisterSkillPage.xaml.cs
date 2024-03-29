﻿using KeyboardKing.core;
using System.Windows;
using System.Windows.Controls;
using Controller;
using Model;
using DatabaseController;

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
            User user = (User)Session.Get("student");
            string tag = ((Button)sender).Tag.ToString();
            DBQueries.AddSkill(tag, user);

            if (tag.Equals("beginner"))
                NavigationController.NavigateToPage(Pages.ExplanationPage);
            else
                NavigationController.NavigateToPage(Pages.ChaptersPage);
        }
    }
}
