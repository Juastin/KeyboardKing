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
                    Session.Add("InfiniteMode", new UList(new object[]{"ThreeLifesMode", 3}));
                    GamemodeController.SetRandomEpisode();
                    NavigationController.NavigateToPage(Pages.InfiniteModePage);
                    break;
                case "OneLifeMode":
                    Session.Add("InfiniteMode", new UList(new object[]{"OneLifeMode", 1}));
                    GamemodeController.SetRandomEpisode();
                    NavigationController.NavigateToPage(Pages.InfiniteModePage);
                    break;
                case "InfiniteMode":
                    Session.Add("InfiniteMode", new UList(new object[]{"InfiniteMode", -1}));
                    GamemodeController.SetRandomEpisode();
                    NavigationController.NavigateToPage(Pages.InfiniteModePage);
                    break;
                default:
                    break;
            }
        }

        public void FetchScores()
        {
            User user = (User)Session.Get("student");
            List<string> result = DBQueries.GetAllGamemodeScores(user.Id);

            // Display the fetched data
            InfiniteModeScore.Content = result[0];
            ThreeLifesModeScore.Content = result[1];
            OneLifeModeScore.Content = result[2];

            Session.Add("GamemodeScores", result);
        }
    }
}
