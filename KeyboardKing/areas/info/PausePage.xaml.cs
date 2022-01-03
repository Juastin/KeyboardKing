using Controller;
using KeyboardKing.core;
using Model;
using System;

namespace KeyboardKing.areas.info
{
    /// <summary>
    /// Interaction logic for MessagePage.xaml
    /// </summary>
    public partial class PausePage : JumpPage
    {
        private Pages _targetLocationBack;
        private Pages _targetLocationForward;

        public PausePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            UList vars = (UList)Session.Get("PausePageInfo");

            TitleLabel.Text = vars.Get<string>(0);
            _targetLocationBack = vars.Get<Pages>(1);
            _targetLocationForward = vars.Get<Pages>(2);
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {

        }
        public void Back(object sender, EventArgs e)
        {
            NavigationController.NavigateToPage(_targetLocationBack);
        }

        public void Forward(object sender, EventArgs e)
        {
            EpisodeController.Exit(_targetLocationForward);
        }
    }
}