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
        private Pages _targetLocation;
        public PausePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            UList vars = (UList)Session.Get("PausePageInfo");

            TitleLabel.Text = vars.Get<string>(0);
            _targetLocation = vars.Get<Pages>(1);
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {

        }
        public void Redirect(object sender, EventArgs e)
        {
            NavigationController.NavigateToPage(_targetLocation);
        }
    }
}