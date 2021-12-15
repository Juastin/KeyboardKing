using KeyboardKing.core;
using Controller;
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
using System.Windows.Shapes;
using Model;

// Source: https://stackoverflow.com/questions/561166/binding-a-wpf-combobox-to-a-custom-list

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchCreatePage.xaml
    /// </summary>
    public partial class MatchCreatePage : JumpPage
    {
        public MatchCreatePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            if (MatchController.CheckIfUserExists())
            {
                MessageBox.Show("Je zit al in een match");
                NavigationController.NavigateToPage(Pages.MatchOverviewPage);
            }

            User user = (User)Session.Get("student");

            CBEpisode.ItemsSource = DBQueries.GetAllEpisodes(user);
            CBEpisode.DisplayMemberPath = "Name";
            CBEpisode.SelectedValuePath = "Id";
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void BCreateMatch(object sender, RoutedEventArgs e)
        {
            if (CBEpisode.SelectedValue != null)
            {
                MatchController.MakeMatch((int)CBEpisode.SelectedValue);
                NavigationController.NavigateToPage(Pages.MatchLobbyPage);
            }
        }

    }
}
