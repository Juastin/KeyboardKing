using System.Collections.Generic;
using System.Windows;
using Controller;
using KeyboardKing.core;
using Model;

namespace KeyboardKing.areas.info
{
    /// <summary>
    /// Interaction logic for ConfirmationPage.xaml
    /// </summary>
    public partial class ConfirmationPage : JumpPage
    {
        private Pages _targetLocation;
        private Pages _previousLocation;

        public ConfirmationPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            UList vars = (UList)Session.Get("ConfirmationPageInfo");

            TitleLabel.Text = vars.Get<string>(0);
            _previousLocation = vars.Get<Pages>(1);
            _targetLocation = vars.Get<Pages>(2);
        }

        public override void OnShadow()
        {
            Dispatcher.Invoke(() =>
            {
                TitleLabel.Text = "";
                _targetLocation = Pages.Empty;
                Session.Remove("ConfirmationPageInfo");
            });
        }

        public override void OnTick()
        {
        }

        private void RedirectApproved(object sender, RoutedEventArgs e)
        {
            if (_previousLocation == Pages.SettingsPage && _targetLocation == Pages.SettingsPage)
                Session.Add("deleteUser", true);
            // This check can probably be improved by creating some sort of musicNavigation controller.
            if (new List<Pages>(){Pages.ChaptersPage, Pages.MatchOverviewPage, Pages.SettingsPage}.Contains(_targetLocation))
                MusicPlayer.PlayNextFrom("menu_music");
            NavigationController.NavigateToPage(_targetLocation);
        }

        private void RedirectDeclined(object sender, RoutedEventArgs e)
        {
            NavigationController.NavigateToPage(_previousLocation);
        }
    }
}
