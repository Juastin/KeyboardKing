﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace Model
{
    public class MatchPageDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Word { get => MatchController.Word; }
        public string WordOverlayWrong { get => MatchController.WordOverlayWrong; }
        public string WordOverlayCorrect { get => MatchController.WordOverlayCorrect; }
        public int LettersTyped { get => MatchController.LettersTyped; }
        public int MaxLetters { get => MatchController.CurrentEpisodeResult?.MaxScore ?? 0; }
        public List<List<string>> OpponentData { get => MatchController.OpponentData; }

        public MatchPageDataContext()
        {
            MatchController.WordChanged += OnWordChanged;
        }

        private void OnWordChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}