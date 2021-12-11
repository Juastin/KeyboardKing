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
            UpdateListView();
            Session.Add("matchId", int.Parse(_matchInfoLoad[0][0]));
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
            DBQueries.SetPlayState(int.Parse(_matchInfoLoad[0][0]), 1);
        }

        private void StartGame()
        {
            this.Dispatcher.Invoke(MatchController.StartGame);
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

            if (int.Parse(_matchInfoLoad[0][10]) == 1) StartGame();

            this.Dispatcher.Invoke(() =>
            {
                int selectedItem = LvMatch.SelectedIndex;
                LvMatch.ItemsSource = items;
                LvMatch.SelectedIndex = selectedItem;
                lEpisodeMatch.Content = _matchInfoLoad[0][2];
                startbtn.Visibility = MatchController.CheckUserIsCreator() ? Visibility.Visible : Visibility.Hidden;
            });
        }

        private void BExitMatch(object sender, EventArgs e)
        {
            _checkIfLeft = true;
            if (MatchController.CheckUserIsCreator())
            {
                if (MatchController.CheckCreatorIsAloneInMatch())
                {
                    MatchController.DeleteMatch();
                    MessageBox.Show("Match is verwijderd");
                }
                else
                {
                    //must change
                    MatchController.UpdateCreatorInMatch();
                }
                NavigationController.NavigateToPage(Pages.MatchOverviewPage);
            }
            else
            {
                MatchController.RemoveUserInMatchProgress();
                NavigationController.NavigateToPage(Pages.MatchOverviewPage);
            }
        }
    }
}
