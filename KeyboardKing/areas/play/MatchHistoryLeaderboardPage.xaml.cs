using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchHistoryLeaderboardPage.xaml
    /// </summary>
    public partial class MatchHistoryLeaderboardPage : JumpPage
    {
        public MatchHistoryLeaderboardPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            int selected_matchid = (int)Session.Get("MatchHistorySelectedMatch");
            Session.Remove("MatchHistorySelectedMatch");

            if (NavigationController.PreviousPage == Pages.MatchResultPage)
            {
                Dispatcher.Invoke(() =>
                {
                    BackButton.Content = "Beëindigen";
                    BackButton.ToPage = Pages.MatchOverviewPage;
                });
            }

            List<List<string>> raw_result = DBQueries.GetAllScoresOrderByHighest(selected_matchid);

            // Prevent non-existed matches from being loaded
            if (raw_result.Count==0) {MessageController.Show(Pages.MessagePage, "Deze match is niet beschikbaar.", Pages.MatchHistoryPage, -1); return;}

            // Parse the received data so the timestamps are readable
            var actual_matches = raw_result.Select(m => new
            {
                EpisodeName = m[0],
                Username = m[1],
                Score = m[2],
                Mistakes = m[3],
                LPM = m[4],
                TimeSpend = ((int)new TimeSpan(long.Parse(m[5])).TotalMinutes) + ":" + (((new TimeSpan(long.Parse(m[5])).TotalSeconds % 60) < 10) ? "0" : "") + ((int)new TimeSpan(long.Parse(m[5])).TotalSeconds % 60),
            }).ToList();

            EpisodeNameLabel.Content = actual_matches[0].EpisodeName;
            MatchHistoryList.ItemsSource = actual_matches;
            MatchHistoryList.Items.Refresh();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }
    }
}
