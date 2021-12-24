using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchHistoryPage.xaml
    /// </summary>
    public partial class MatchHistoryPage : JumpPage
    {
        private DateTime _tickCheck {get;set;} = DateTime.MinValue;
        private object _currentHistory {get;set;}

        public MatchHistoryPage(MainWindow w) : base(w)
        {
            InitializeComponent();
            MatchHistoryList.AddHandler(GridViewRowPresenter.MouseLeftButtonDownEvent, new RoutedEventHandler(MatchHistory_InfoClick), true);
        }

        public override void OnLoad()
        {
            // FETCH ITEMS
            DateTime now = DateTime.Now;
            if (_tickCheck.AddMinutes(5) < now || ((Session.Get("FetchMatchHistory") is not null && (bool)Session.Get("FetchMatchHistory")) ))
            {
                _tickCheck = now;
                Session.Remove("FetchMatchHistory");
                LoadAllMatches();
            }
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
                object actual_matches;

                if (raw_matches.Count>0)
                {
                    // Parse the received data so the timestamps are readable
                    actual_matches = raw_matches.Select(m => new
                    {
                        Matchid = m[0],
                        Time = DateFormatter.TimeAgoNL(DateTime.Parse(m[1])),
                        Score = m[2],
                        Mistakes = m[3],
                        LPM = m[4],
                        TimeSpend = ((int)new TimeSpan(long.Parse(m[5])).TotalMinutes) + ":" + (((new TimeSpan(long.Parse(m[5])).TotalSeconds % 60) < 10) ? "0" : "") + ((int)new TimeSpan(long.Parse(m[5])).TotalSeconds % 60),
                    }).ToList();
                } else
                {
                    // IF NO DATA IS PRESENT, DISPLAY A PLACEHOLDER
                    actual_matches = new List<object>(){new {Matchid = "-1", Time = "Je hebt nog geen match gespeeld.", Score = "", Mistakes = "", LPM = "", TimeSpend = ""}};
                }

                _currentHistory = actual_matches;
                MatchHistoryList.ItemsSource = (System.Collections.IEnumerable)actual_matches;
                MatchHistoryList.SelectedValuePath = "Matchid";
                MatchHistoryList.Items.Refresh();
            });
        }

        private void MatchHistory_InfoClick(object sender, RoutedEventArgs e)
        {
            if (MatchHistoryList.SelectedValue is not null)
            {
                int selected_matchid = int.Parse((string)MatchHistoryList.SelectedValue);

                if (selected_matchid!=-1)
                {
                    Session.Add("MatchHistorySelectedMatch", selected_matchid);
                    NavigationController.NavigateToPage(Pages.MatchHistoryLeaderboardPage);
                    MatchHistoryList.SelectedIndex = -1;
                    return;
                }
            }
        }
    }
}
