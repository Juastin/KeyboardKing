﻿using Controller;
using KeyboardKing.core;
using Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for EpisodeReadyUpPage.xaml
    /// </summary>
    public partial class EpisodeReadyUpPage : JumpPage
    {
        public EpisodeReadyUpPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            MusicPlayer.PlayNextFrom("waiting");
            this.UserInput.Focus();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                NavigationController.NavigateToPage(Pages.EpisodePage);
            }
        }

        /// <summary>
        /// <para>Event that fires each time when focus of window has been lost.</para>
        /// This way the UserInput field is always focused and can always be filled in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            UserInput.Focus();
        }

        /// <summary>
        /// <para>Event that fires when the UserInput changes.</para>
        /// <para>This input is then passed to the method <see cref="EpisodeController.CheckInput"/></para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserInput.Clear();
        }
    }
}
