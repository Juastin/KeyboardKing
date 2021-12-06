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

            UList user = (UList)Session.Get("student");
            List<List<string>> episodes = DBQueries.GetAllEpisodes(user);
            int counter = 0;
            List<EpisodeData> listData = new List<EpisodeData>();

            while (counter < episodes.Count)
            {
                listData.Add(new EpisodeData { EpisodeId = int.Parse(episodes[counter][3]), EpisodeName = episodes[counter][2] });
                counter++;
            }
            CBEpisode.ItemsSource = listData;
            CBEpisode.DisplayMemberPath = "EpisodeName";
            CBEpisode.SelectedValuePath = "EpisodeId";
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void BCreateMatch(object sender, RoutedEventArgs e)
        {
            MatchController.MakeMatch((int) CBEpisode.SelectedValue);
            NavigationController.NavigateToPage(Pages.MatchLobbyPage);
        }

    }
}
