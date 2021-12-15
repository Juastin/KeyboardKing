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
        private bool _checkIfLeft;

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
            _checkIfLeft = false;
            UpdateListView();
        }

        public override void OnShadow()
        {
            
        }

        public override void OnTick()
        {
            if (!_checkIfLeft)
            {
                UpdateListView();
            }
        }

        private void EpOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            MatchController.SetPlayingState();
        }

        private void StartGame()
        {
            this.Dispatcher.Invoke(MatchController.StartGame);
        }


        private void UpdateListView()
        {
            _matchInfoLoad = MatchController.GetMatchProgressInfo();
            List<MatchLobbyData> items = new List<MatchLobbyData>();
            int counter = 0;
            while (counter < _matchInfoLoad.Count)
            {
                string symbol = MatchController.CheckUserIsCreator(_matchInfoLoad[counter][11]) ? "\u2654" : string.Empty;
                items.Add(new MatchLobbyData() { Logo = symbol, Username = $"{_matchInfoLoad[counter][1]}" });;
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

                GridViewColumn lastColumn = ((GridView)LvMatch.View).Columns.Last();
                ResizeGridViewColumn(lastColumn);
            });
        }

        // Resize specific column to the width of the largest column item
        private void ResizeGridViewColumn(GridViewColumn column)
        {
            if (double.IsNaN(column.Width))
            {
                column.Width = column.ActualWidth;
            }

            column.Width = double.NaN;
        }

            private void BExitMatch(object sender, EventArgs e)
        {
            _checkIfLeft = true;
            if (MatchController.CheckUserIsCreator())
            {
                if (MatchController.CheckCreatorIsAloneInMatch())
                {
                    MatchController.DeleteMatch();
                    MessageController.Show(Pages.MessagePage, "De match is verwijderd", Pages.MatchOverviewPage, -1);
                    return;
                }
                else
                {
                    MatchController.UpdateCreatorInMatch();
                    MatchController.RemoveUserInMatchProgress();
                }
            }
            else
            {
                MatchController.RemoveUserInMatchProgress();
            }
            NavigationController.NavigateToPage(Pages.MatchOverviewPage);
        }
    }
}
