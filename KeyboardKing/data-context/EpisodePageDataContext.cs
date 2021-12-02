﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class EpisodePageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Word { get => EpisodeController.Word; }
        public string WordOverlayWrong { get => EpisodeController.WordOverlayWrong; }
        public string WordOverlayCorrect { get => EpisodeController.WordOverlayCorrect; }
        public int LettersTyped { get => EpisodeController.LettersTyped; }
        public int MaxLetters { get => EpisodeController.CurrentEpisodeResult?.MaxScore ?? 0; }
        public EpisodePageDataContext()
        {
            EpisodeController.WordChanged += OnWordChanged;
        }

        private void OnWordChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
