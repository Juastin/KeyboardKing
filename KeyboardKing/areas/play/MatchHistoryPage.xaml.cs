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
    /// Interaction logic for MatchHistoryPage.xaml
    /// </summary>
    public partial class MatchHistoryPage : JumpPage
    {
        private DateTime _tickCheck {get;set;} = DateTime.Now;
        private List<List<string>> _currentHistory {get;set;}

        public MatchHistoryPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            LoadAllMatches();
            MatchHistoryList.AddHandler(GridViewRowPresenter.MouseLeftButtonDownEvent, new RoutedEventHandler(MatchHistory_InfoClick), true);
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        public void LoadAllMatches()
        {
            Dispatcher.Invoke(() =>
            {
                User user = (User)Session.Get("student");
                List<List<string>> raw_matches = DBQueries.GetMatchProgressesWithUser(user.Id);

                // Parse the received data so the timestamps are readable
                List<List<string>> actual_matches = new List<List<string>>();
                foreach (List<string> part in raw_matches)
                {
                    string matchid = part[0];
                    string time = DateFormatter.TimeAgoNL(DateTime.Parse(part[1]));
                    string score = part[2];
                    string mistakes = part[3];
                    string lpm = part[4];
                    string timespend = ((int)new TimeSpan(long.Parse(part[5])).TotalMinutes) + ":" + (((new TimeSpan(long.Parse(part[5])).TotalSeconds % 60)<10) ? "0" : "") + ((int)new TimeSpan(long.Parse(part[5])).TotalSeconds % 60);
                    actual_matches.Add(new List<string>(){matchid, time, score, mistakes, lpm, timespend});
                }

                // IF NO DATA IS PRESENT, DISPLAY A PLACEHOLDER
                actual_matches = (actual_matches.Count>0) ? actual_matches : new List<List<string>>(){new List<string>{"", "Je hebt nog geen match gespeeld.", ""}};
                _currentHistory = actual_matches;

                MatchHistoryList.ItemsSource = actual_matches;
                MatchHistoryList.Items.Refresh();
            });
        }

        private void MatchHistory_InfoClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_currentHistory[MatchHistoryList.SelectedIndex][0]);
        }
    }
}
