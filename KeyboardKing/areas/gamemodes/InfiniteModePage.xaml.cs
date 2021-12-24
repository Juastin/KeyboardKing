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
        private int? _allowedMistakes {get;set;}
        private string? _selectedGamemode {get;set;}

        public InfiniteModePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            _selectedGamemode = ((UList)Session.Get("InfiniteMode")).Get<string>(0);
            _allowedMistakes = ((UList)Session.Get("InfiniteMode")).Get<int>(1);

            if (!EpisodeController.IsStarted)
                MusicPlayer.PlayNextFrom("intense_music");

            EpisodeController.Start();
            this.UserInput.Focus();
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {
        }

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
                EpisodeController.CheckInput(txt[0]);
            }
            this.UserInput.Clear();

            // Exit the gamemode if the set amount of mistakes was made.
            if (EpisodeController.CurrentEpisodeResult.Mistakes==_allowedMistakes)
            {
                EpisodeController.StopAndSetEpisodeResult();
            }
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
            EpisodeController.Pause();
        }

        private void ExitGamemode()
        {
            Session.Remove("InfiniteModeAllowedMistakes");
        }
    }
}
