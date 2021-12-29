using Controller;
using KeyboardKing.core;
using Model;
using System;
using System.Collections.Generic;

namespace KeyboardKing.areas.info
{
    /// <summary>
    /// Interaction logic for MessagePage.xaml
    /// </summary>
    public partial class MessagePage : JumpPage
    {
        private Pages TargetLocation {get;set;}
        
        private int AutoRedirectTime {get;set;}

        public MessagePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            UList vars = (UList)Session.Get("MessagePageInfo");

            TitleLabel.Text = vars.Get<string>(0);
            TargetLocation = vars.Get<Pages>(1);
            AutoRedirectTime = vars.Get<int>(2);

            UpdateButtonText();
        }

        public override void OnShadow()
        {
            Dispatcher.Invoke(() =>
            {
                TitleLabel.Text = "";
                TargetLocation = Pages.Empty;
            });
            Session.Remove("MessagePageInfo");
        }

        public override void OnTick()
        {
            if (AutoRedirectTime!=-1)
            {
                // Redirect when time runs out
                if (AutoRedirectTime==0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        NavigationController.NavigateToPage(TargetLocation);
                    });
                }
                UpdateButtonText();
                AutoRedirectTime--;
            }
        }

        public void UpdateButtonText()
        {
            Dispatcher.Invoke(() =>
            {
                RedirectButton.Content = (AutoRedirectTime<30&&AutoRedirectTime>-1) ? $"Oke ({AutoRedirectTime-1})" : "Oke";
            });
        }

        public void Redirect(object sender, EventArgs e)
        {
            if (new List<Pages>(){Pages.ChaptersPage, Pages.MatchOverviewPage, Pages.SettingsPage, Pages.GamemodesOverviewPage}.Contains(TargetLocation))
                MusicPlayer.PlayNextFrom("menu_music");

            NavigationController.NavigateToPage(TargetLocation);
        }
    }
}
