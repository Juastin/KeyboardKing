using Controller;
using KeyboardKing.core;
using Model;
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

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchOverviewPage.xaml
    /// </summary>
    public partial class MatchOverviewPage : JumpPage
    {
        private DateTime _tickCheck {get;set;} = DateTime.Now;
        private bool _isInMatch;

        public MatchOverviewPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            LoadAllMatches();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
            DateTime now = DateTime.Now;
            if (_tickCheck.AddSeconds(5) < now) // if 5 seconds have passed
            {
                _tickCheck = now;
                LoadAllMatches();
                _isInMatch = MatchController.CheckIfUserExists();
            }
        }

        public void LoadAllMatches()
        {
            Dispatcher.Invoke(() =>
            {
                List<Match> matches = DBQueries.GetAllActiveMatches();
                MatchOverview.ItemsSource = matches;
                MatchOverview.Items.Refresh();
            });
        }

        private void MatchOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            if (!_isInMatch)
            {
                Button button = (Button)sender;
                if (button.DataContext is Match match)
                {
                    MatchController.Initialize(match);
                    MatchController.AddUserInMatchProgress();
                    NavigationController.NavigateToPage(Pages.MatchLobbyPage);
                }
            }
            else { MessageBox.Show("Je zit al in een match"); }
        }
    }
}
