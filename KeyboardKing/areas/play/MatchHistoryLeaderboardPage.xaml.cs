using Controller;
using DatabaseController;
using KeyboardKing.core;
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

            List<List<string>> raw_result = DBQueries.GetAllScoresOrderByHighest(selected_matchid);
            // Parse the received data so the timestamps are readable

            /**
             * Easy way of creating an object without creating a whole new one in Model
             * `actual_matches` is now a List of an anonymous type.
             * You're able to call `actual_matches[0].EpisodeName` on it this way instead of `actualmatches[0][0]`
             * 
             * If you want to get rid of these changes just uncomment the old code and replace the bindings back in `MatchHistoryLeaderboardPage.xaml`
             */
            var actual_matches = raw_result.Select(m => new
            {
                EpisodeName = m[0],
                Username = m[1],
                Score = m[2],
                Mistakes = m[3],
                LPM = m[4],
                TimeSpend = ((int)new TimeSpan(long.Parse(m[5])).TotalMinutes) + ":" + (((new TimeSpan(long.Parse(m[5])).TotalSeconds % 60) < 10) ? "0" : "") + ((int)new TimeSpan(long.Parse(m[5])).TotalSeconds % 60),
            }).ToList();

            /** Old code, remove when not needed. Still here so you can easily revert my changes
            List<List<string>> actual_matches = new List<List<string>>();
            foreach (List<string> part in raw_result)
            {
                string episode_name = part[0];
                string username = part[1];
                string score = part[2];
                string mistakes = part[3];
                string lpm = part[4];
                string timespend = ((int)new TimeSpan(long.Parse(part[5])).TotalMinutes) + ":" + (((new TimeSpan(long.Parse(part[5])).TotalSeconds % 60)<10) ? "0" : "") + ((int)new TimeSpan(long.Parse(part[5])).TotalSeconds % 60);
                actual_matches.Add(new List<string>(){episode_name, username, score, mistakes, lpm, timespend});
            }

            EpisodeNameLabel.Content = actual_matches[0][0];
            */

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
