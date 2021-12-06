﻿using KeyboardKing.core;
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
        private DateTime _tickCheck { get; set; } = DateTime.Now;

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
            List<List<string>> matchInfo = DBQueries.GetMatchProgress(MatchController.GetMatchId());
            lEpisodeMatch.Content = matchInfo[0][2];
            // check if creatorid == useris -> button is visible
            List<MatchLobbyData> items = new List<MatchLobbyData>();
            int counter = 0;
            while (counter < matchInfo.Count)
            {
                items.Add(new MatchLobbyData() { Username = matchInfo[counter][2] });
                counter++;
            }
            LvMatch.ItemsSource = items;
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
                List<List<string>> matchInfo = DBQueries.GetMatchProgress(MatchController.GetMatchId());
                List<MatchLobbyData> items = new List<MatchLobbyData>();
                int counter = 0;
                while (counter < matchInfo.Count)
                {
                    items.Add(new MatchLobbyData() { Username = matchInfo[counter][2] });
                    counter++;
                }
                LvMatch.ItemsSource = null;
                LvMatch.ItemsSource = items;
            }
        }

        private void BStartMatch(object sender, EventArgs e)
        {
            MessageBox.Show("Start de match");
        }

    }
}
