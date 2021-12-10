using Controller;
using KeyboardKing.core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchPlayingPage.xaml
    /// </summary>
    public partial class MatchPlayingPage : JumpPage
    {
        /// <summary>
        /// Controller for <see cref="MatchPlayingPage"/>
        /// </summary>
        /// <param name="w"></param>

        public int increment { get; set; }
        private DispatcherTimer dt = new DispatcherTimer();

        public MatchPlayingPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            Initialize();
            MusicPlayer.PlayNextFrom("intense_music");
            MatchController.Start();
            this.UserInput.Focus();
            UpdateOpponentProgress();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
            MatchController.MultiplayerFetch();
            UpdateOpponentProgress();
        }

        public void UpdateOpponentProgress()
        {
            this.Dispatcher.Invoke(() =>
            {
                OpponentListBox.ItemsSource = MatchController.OpponentData;
                OpponentListBox.Items.Refresh();
            });
        }

        private void Initialize()
        {
            TimerTextBox.Text = "00:00";
            points.Text = "0p";
            MatchController.Points = 0;
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
                MatchController.CheckInput(txt[0]);
            }
            points.Text = MatchController.Points.ToString() + "p";
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

        private void TimerTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            increment = 0;
            dt.Tick -= dtTicker;
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }

        private void dtTicker(object sender, EventArgs e)
        {
            increment++;
            TimeSpan result = TimeSpan.FromSeconds(increment);
            TimerTextBox.Text = result.ToString("mm':'ss");
        }

        private void ButtonExit(object sender, EventArgs e) // leave match set state 2 or remove match
        {
            MatchController.RemoveUserInMatchProgress();

            int matchId = MatchController.GetMatchId();

            if (!DBQueries.GetMatchProgress(matchId).Any())
            {
                DBQueries.DeleteMatch(matchId);
            }

            MusicPlayer.PlayNextFrom("menu_music");
            NavigationController.NavigateToPage(Pages.ChaptersPage);
        }
    }
}
