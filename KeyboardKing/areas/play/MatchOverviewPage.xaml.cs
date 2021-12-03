﻿using Controller;
using KeyboardKing.core;
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
            }
        }

        public void LoadAllMatches()
        {
            Dispatcher.Invoke(() =>
            {
                List<List<string>> Matches = DBQueries.GetAllActiveMatches();
                MatchOverview.ItemsSource = Matches;
                MatchOverview.Items.Refresh();
            });
        }

        private void MatchOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.DataContext is List<string>)
            {
                List<string> row = (List<string>)button.DataContext;
                MessageBox.Show(row[2]);
                //Navigate("MatchLobbyPage");
            }
        }
    }
}
