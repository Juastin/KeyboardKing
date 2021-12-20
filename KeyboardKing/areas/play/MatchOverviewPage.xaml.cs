using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
                    if (MatchController.CheckIfMatchExists(match.Id))
                    {
                        MatchController.Initialize(match);
                        MatchController.AddUserInMatchProgress();
                        NavigationController.NavigateToPage(Pages.MatchLobbyPage);
                    }
                    else
                    {
                        MessageController.Show(Pages.MessagePage, "De match is verwijderd", Pages.MatchOverviewPage, 5);
                    }
                }
            }
            else { MessageController.Show(Pages.MessagePage, "De match is verwijderd", Pages.MatchOverviewPage, 5); }
        }
    }
}
