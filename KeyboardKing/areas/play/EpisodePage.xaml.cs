using KeyboardKing.core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for EpisodePage.xaml
    /// </summary>
    public partial class EpisodePage : JumpPage
    {
        /// <summary>
        /// Constructor of <see cref="EpisodePage"/>
        /// </summary>
        /// <param name="w"></param>
        /// 

        public EpisodePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            if (!EpisodeController.IsStarted)
                MusicPlayer.PlayNextFrom("intense_music");

            EpisodeController.Start();
            UpdateTimerView();
            this.UserInput.Focus();
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {
            UpdateTimerView();
        }

        private void UpdateTimerView() => Dispatcher.Invoke(() => TimerTextBox.Text = EpisodeController.GetTimeFormat());

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
            points.Text = EpisodeController.Points.ToString() + "p";
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
            EpisodeController.Pause();

            //MusicPlayer.PlayNextFrom("menu_music");
            //NavigationController.NavigateToPage(Pages.ChaptersPage);
        }
    }
}
