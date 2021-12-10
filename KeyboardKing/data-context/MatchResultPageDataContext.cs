﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class MatchResultPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Score { get => $"{EpisodeController.CurrentEpisodeResult?.Score ?? 0}p"; }
        public int Mistakes { get => EpisodeController.CurrentEpisodeResult?.Mistakes ?? 0; }
        public string ScorePercentage { get => $"{EpisodeController.CurrentEpisodeResult?.ScorePercentage ?? 0}%"; }
        public string Time { get => FormatTimespan(); }
        public double LettersPerMinute { get => EpisodeController.CurrentEpisodeResult?.LettersPerMinute ?? 0; }

        public string Winnaar1 { get => MatchController.Winnaar1; }
        public string Winnaar2 { get => MatchController.Winnaar2; }
        public string Winnaar3 { get => MatchController.Winnaar3; }

        public string Score1 { get => MatchController.Score1; }
        public string Score2 { get => MatchController.Score2; }
        public string Score3 { get => MatchController.Score3; }
        public MatchResultPageDataContext()
        {
            EpisodeController.EpisodeFinished += OnRefresh;
            MatchController.Refresh += OnRefresh;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        private string FormatTimespan()
        {
            TimeSpan span = EpisodeController.CurrentEpisodeResult?.Time ?? TimeSpan.Zero;
            return string.Format("{0}min {1}sec",
                (int)span.TotalMinutes,
                span.Seconds);
        }
    }
}