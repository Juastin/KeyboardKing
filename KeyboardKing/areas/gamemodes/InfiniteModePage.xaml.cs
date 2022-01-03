using Controller;
using KeyboardKing.core;
using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyboardKing.areas.gamemodes
{
    /// <summary>
    /// Interaction logic for InfiniteModePage.xaml
    /// </summary>
    public partial class InfiniteModePage : JumpPage
    {
        public InfiniteModePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            string selected_gamemode = ((UList)Session.Get("InfiniteMode")).Get<string>(0);
            int allowed_mistakes = ((UList)Session.Get("InfiniteMode")).Get<int>(1);

            if (!GamemodeController.IsStarted)
            {
                GamemodeController.Initialise(allowed_mistakes, selected_gamemode);
            }

            if (!EpisodeController.IsStarted)
                MusicPlayer.PlayNextFrom("intense_music");

            if (selected_gamemode=="InfiniteMode") {LifeLabel.Visibility = Visibility.Hidden;}
            else {LifeLabel.Visibility = Visibility.Visible;}

            EpisodeController.Start();
            UpdateTimerView();
            UpdateHearts();

            this.UserInput.Focus();
        }

        public override void OnShadow()
        {
            if (!GamemodeController.IsStarted)
            {
                Dispatcher.Invoke(() => ScoreTextBox.Text = "0");
                MusicPlayer.Stop();
                AudioPlayer.Play(AudioPlayer.Sound.congratulations);
            }
        }

        public override void OnTick()
        {
            UpdateTimerView();
        }

        private void UpdateTimerView() => Dispatcher.Invoke(() => TimerTextBox.Text = EpisodeController.GetTimeFormat());
        private void UpdateScoreView() => Dispatcher.Invoke(() => ScoreTextBox.Text = GamemodeController.Score.ToString());

        /// <summary>
        /// <para>Event that fires each time when focus of window has been lost.</para>
        /// This way the UserInput field is always focused and can always be filled in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UserInput.Focus();
        }

        /// <summary>
        /// <para>Event that fires when the UserInput changes.</para>
        /// Method gets the last character in the UserInput.
        /// <para>This input is then passed to the method <see cref="EpisodeController.CheckInput"/></para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = this.UserInput.Text;

            if (txt.Length > 0)
            {
                // Increase score if the typed character matches the expected character.
                if (EpisodeController.IsInputCorrect(txt[0]))
                {
                    GamemodeController.Score++;
                    UpdateScoreView();
                } else
                {
                    GamemodeController.Mistakes++;
                }
                EpisodeController.CheckInput(txt[0]);
            }

            GamemodeController.Checks();
            UpdateHearts();

            this.UserInput.Clear();
        }

        /// <summary>
        /// Prevents tab from interfering with the focus on the TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventTab_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                e.Handled = true;
        }

        private void ButtonPause(object sender, EventArgs e)
        {
            EpisodeController.Pause(Pages.InfiniteModePage, Pages.GamemodesOverviewPage);
        }

        private void UpdateHearts()
        {
            if (GamemodeController.AllowedMistakes>0)
            {
                Dispatcher.Invoke(() => {
                    LifeLabel.Content = "";
                    for (int i = 0; i<GamemodeController.AllowedMistakes-GamemodeController.Mistakes; i++)
                    {
                        LifeLabel.Content += "♡";
                    }
                });
            }
        }
    }
}
