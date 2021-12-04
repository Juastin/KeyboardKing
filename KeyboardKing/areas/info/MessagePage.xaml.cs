using Controller;
using KeyboardKing.core;
using Model;
using System;

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
            object[] vars = (object[])Session.Get("MessagePageInfo");

            TitleLabel.Text = (string)vars[0];
            TargetLocation = (Pages)vars[1];
            AutoRedirectTime = (int)vars[2]!=0 ? (int)vars[2] : 9999999;

            UpdateButtonText();
        }

        public override void OnShadow()
        {
            Dispatcher.Invoke(() =>
            {
                TitleLabel.Text = "";
                TargetLocation = Pages.Empty;
                Session.Remove("MessagePageInfo");
            });

        }

        public override void OnTick()
        {
            // Redirect when time runs out
            if (AutoRedirectTime<0)
            {
                Dispatcher.Invoke(() =>
                {
                    NavigationController.NavigateToPage(TargetLocation);
                });
            }

            UpdateButtonText();
            AutoRedirectTime--;
        }

        public void UpdateButtonText()
        {
            Dispatcher.Invoke(() =>
            {
                RedirectButton.Content = AutoRedirectTime<30 ? $"Oke ({AutoRedirectTime})" : "Oke";
            });
        }

        public void Redirect(object sender, EventArgs e)
        {
            NavigationController.NavigateToPage(TargetLocation);
        }
    }
}
