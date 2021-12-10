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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchLobbyPage.xaml
    /// </summary>
    public partial class MatchLobbyPage : JumpPage
    {
        //private DateTime _tickCheck { get; set; } = DateTime.Now;

        private List<List<string>> _matchInfoLoad;

        private bool _checkIfLeft = false;

        /// <summary>
        /// Controller for <see cref="MatchLobbyPage"/>
        /// </summary>
        /// <param name="w"></param>
        public MatchLobbyPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            _matchInfoLoad = DBQueries.GetMatchProgress(MatchController.GetMatchId());
            lEpisodeMatch.Content = _matchInfoLoad[0][2];
            UpdateListView();
            if (!MatchController.CheckUserIsCreator()) { startbtn.Visibility = Visibility.Hidden; }
            else { startbtn.Visibility = Visibility.Visible; }
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {
            if(!_checkIfLeft)
            {
                UpdateListView();
            }
        }

        private void EpOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            // query die checkt als iedereen ready is

            DBQueries.SetPlayState(int.Parse(_matchInfoLoad[0][0]), 1);
        }

        private void StartGame()
        {
            this.Dispatcher.Invoke(() =>
            {
                MatchController.EpisodeFinished += OnEpisodeFinished;
                Episode episode = MatchController.ParseEpisode(int.Parse(_matchInfoLoad[0][9]));
                MatchController.Initialise(episode);
                NavigationController.NavigateToPage(Pages.MatchPlayingPage);
            });  
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
        {
            MatchController.EpisodeFinished -= OnEpisodeFinished;
            DBQueries.SetPlayState(int.Parse(_matchInfoLoad[0][0]), 2);
            NavigationController.NavigateToPage(Pages.MatchResultPage);
        }

        private void SetPlayerReady(object sender, RoutedEventArgs e)
        {
            if(ready.Content.ToString() == "Ready?" || ready.Content.ToString() == "No")
            {
                ready.Content = "Yes";
            } else
            {
                ready.Content = "No";
            }
        }

        private void UpdateListView()
        {
            _matchInfoLoad = MatchController.GetMatchProgressInfo();
            List<MatchLobbyData> items = new List<MatchLobbyData>();
            int counter = 0;
            while (counter < _matchInfoLoad.Count)
            {
                items.Add(new MatchLobbyData() { Username = _matchInfoLoad[counter][1] });
                counter++;
            }

            if (int.Parse(_matchInfoLoad[0][10]) == 1)
            {
                StartGame();
            }

            this.Dispatcher.Invoke(() =>
            {
                int SelectedItem = LvMatch.SelectedIndex;
                LvMatch.ItemsSource = items;
                LvMatch.SelectedIndex = SelectedItem;          
            });
        }

        private void BExitMatch(object sender, EventArgs e)
        {
            if (MatchController.CheckUserIsCreator())
            {
                if (MatchController.CheckCreatorIsAloneInMatch())
                {
                    _checkIfLeft = true;
                    MatchController.DeleteMatch();
                    MessageBox.Show("Match is deleted");
                    NavigationController.NavigateToPage(Pages.MatchOverviewPage);
                }
                else { MessageBox.Show("Je kan momenteel niet de match verlaten. Je bent de creator"); }
            }
            else
            {
                MatchController.RemoveUserInMatchProgress();
                NavigationController.NavigateToPage(Pages.MatchOverviewPage);
            }
        }
    }
}
