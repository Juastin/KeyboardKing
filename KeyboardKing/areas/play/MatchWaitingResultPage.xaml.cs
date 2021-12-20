using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchWaitingResultPage.xaml
    /// </summary>
    public partial class MatchWaitingResultPage : JumpPage
    {
        public MatchWaitingResultPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            User student = (User)Session.Get("student");
            int match_id = MatchController.GetMatchId();
            DBQueries.UpdateMatchProgress(100, student.Id, match_id);
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
                NavigationController.NavigateToPage(Pages.MatchResultPage);
            }
            });
        }
    }
}