using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace KeyboardKing.areas.gamemodes
{
    /// <summary>
    /// Interaction logic for GamemodesOverviewPage.xaml
    /// </summary>
    public partial class GamemodesOverviewPage : JumpPage
    {
        public GamemodesOverviewPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            if (Session.Get("FetchGamemodeScores") is not null && (bool)Session.Get("FetchGamemodeScores"))
            {
                Session.Remove("FetchGamemodeScores");
                FetchScores();
            }
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        public void SelectGamemode(object sender, RoutedEventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "ThreeLifesMode":
                    //MessageController.Show(Model.Pages.MessagePage, "Deze uitdaging is nog niet beschikbaar.", Model.Pages.GamemodesOverviewPage, 10);
                    Session.Add("InfiniteMode", new UList(new object[]{"ThreeLifesMode", 3}));
                    InfiniteModeController.SetRandomEpisode();
                    NavigationController.NavigateToPage(Pages.InfiniteModePage);
                    break;
                case "InfiniteMode":
                    Session.Add("InfiniteMode", new UList(new object[]{"InfiniteMode", -1}));
                    InfiniteModeController.SetRandomEpisode();
                    NavigationController.NavigateToPage(Pages.InfiniteModePage);
                    break;
                default:
                    break;
            }
        }

        public void FetchScores()
        {
            User user = (User)Session.Get("student");
            List<List<string>> result = DBQueries.GetAllGamemodeScores(user.Id);

            // Display the fetched data
            InfiniteModeScore.Content = result[0][0];
            ThreeLifesModeScore.Content = result[0][1];

            Session.Add("GamemodeScores", result);
        }
    }
}
