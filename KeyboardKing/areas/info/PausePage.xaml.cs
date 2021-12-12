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
        public PausePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {

        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {

        }
        public void Redirect(object sender, EventArgs e)
        {
            NavigationController.NavigateToPage(TargetLocation);
        }
    }
}