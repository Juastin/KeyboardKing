﻿using KeyboardKing.core;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchLobbyPage.xaml
    /// </summary>
    public partial class MatchLobbyPage : JumpPage
    {
        private List<MatchProgress> _matchInfoLoad;
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
            MatchController.SetPlayingState(MatchState.Started);
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
                string symbol = MatchController.CheckUserIsCreator(_matchInfoLoad[counter].User.Id) ? "\u2654" : string.Empty;
                items.Add(new MatchLobbyData() { Logo = symbol, Username = $"{_matchInfoLoad[counter].User.Username}" });;
                counter++;
            }

            if (MatchController.CurrentMatch.State == MatchState.Started) StartGame();

            this.Dispatcher.Invoke(() =>
            {
                int selectedItem = LvMatch.SelectedIndex;
                LvMatch.ItemsSource = items;
                LvMatch.SelectedIndex = selectedItem;
                lEpisodeMatch.Content = MatchController.CurrentMatch.Episode.Name;
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
