using KeyboardKing.core;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchLobbyPage.xaml
    /// </summary>
    public partial class MatchLobbyPage : JumpPage
    {
        private DateTime _tickCheck { get; set; } = DateTime.Now;

        private List<List<string>> _matchInfoLoad;


        /// <summary>
        /// Controller for <see cref="MatchLobbyPage"/>
        /// </summary>
        /// <param name="w"></param>
        public MatchLobbyPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            _matchInfoLoad = DBQueries.GetMatchProgress(MatchController.GetMatchId());
            lEpisodeMatch.Content = _matchInfoLoad[0][2];
            UpdateListView();
            //TODO: check if creatorid == userid -> startmatchbutton is visible for creator
            int state = 5;
            if (_matchInfoLoad[0][10].Equals(state.ToString()){
                EpOverview_PlayClick(new object(), new RoutedEventArgs());
            }    
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {
            DateTime now = DateTime.Now;
            if (_tickCheck.AddSeconds(5) < now) // if 5 seconds have passed
            {
                _tickCheck = now;
                UpdateListView();
            }
        }

        private void EpOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            // query die checkt als iedereen ready is

            // set state to 5 om potje te starten

            bool startGame = DBQueries.SetPlayState(int.Parse(_matchInfoLoad[0][0]), 5);

            if(startGame == true)
            {
                MatchController.EpisodeFinished += OnEpisodeFinished;
                Episode episode = MatchController.ParseEpisode(int.Parse(_matchInfoLoad[0][9]));
                MatchController.Initialise(episode);
                NavigationController.NavigateToPage(Pages.MatchPlayingPage);
            }
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
        {
            MatchController.EpisodeFinished -= OnEpisodeFinished;
            NavigationController.NavigateToPage(Pages.MatchResultPage);
        }

        private void SetPlayerReady(object sender, RoutedEventArgs e)
        {
            if(ready.Content.ToString() == "Ready?" || ready.Content.ToString() == "No")
            {
                ready.Content = "Yes";
            } else
            {
                ready.Content = "No";
            }
        }


        private void BExitMatch(object sender, EventArgs e)
        {
            MessageBox.Show("Verlaat de Match");
        }

        private void UpdateListView()
        {
            List<List<string>> matchInfo = DBQueries.GetMatchProgress(MatchController.GetMatchId());
            List<MatchLobbyData> items = new List<MatchLobbyData>();
            int counter = 0;
            while (counter < matchInfo.Count)
            {
                items.Add(new MatchLobbyData() { Username = matchInfo[counter][1] });
                counter++;
            }
            this.Dispatcher.Invoke(() =>
            {
                LvMatch.ItemsSource = items;
            });
            //LvMatch.ItemsSource = null;
            
        }
    }
}
