using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;
using System.Windows;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchWaitingResultPage.xaml
    /// </summary>
    public partial class MatchWaitingResultPage : JumpPage
    {
        private int _timeLeft { get; set; }

        public MatchWaitingResultPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            User student = (User)Session.Get("student");
            int match_id = MatchController.GetMatchId();
            DBQueries.UpdateMatchProgress(100, student.Id, match_id);

            if (MatchController.CheckUserIsCreator() && !MatchController.CheckIfEverybodyDone())
            {
                StopMatch.Visibility = Visibility.Visible;
                _timeLeft = 30;
                UpdateButtonText();
            }
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
            Dispatcher.Invoke(() =>
            {
                if (MatchController.CheckIfEverybodyDone())
                {
                    MatchController.SetPlayingState(MatchState.Finished);
                    NavigationController.NavigateToPage(Pages.MatchResultPage);
                }
            });

            if (_timeLeft > -1)
            {
                UpdateButtonText();
                _timeLeft--;
            }
        }

        public void UpdateButtonText()
        {
            Dispatcher.Invoke(() =>
            {
                StopMatch.Content = (_timeLeft > 1) ? $"Stop match ({_timeLeft - 1})" : "Stop match";
            });
        }

        private void StopMatch_Click(object sender, RoutedEventArgs e)
        {
            if (_timeLeft < 1)
            {
                MatchController.SetPlayingState(MatchState.Finished);
                //MessageBox.Show("Stop match logics");
            }
        }
    }
}