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
using System.Windows.Shapes;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchCreatePage.xaml
    /// </summary>
    public partial class MatchCreatePage : JumpPage
    {
        public MatchCreatePage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            // ! check if user is already in match/lobby
            string[] User = (string[])Session.Get("student");
            List<List<string>> Episodes = DBQueries.GetAllEpisodes(User);
            int counter = 0;
            while (counter < Episodes.Count)
            {
                if (counter % 10 == 0) { CBChapter.Items.Add(Episodes[counter][0]); }
                //episode.Items.Add($"Episode {Episodes[counter][1]} ({Episodes[counter][2]})");
                if (counter < 10) CBEpisode.Items.Add(Episodes[counter][1]);
                counter++;
            }
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void BCreateMatch(object sender, RoutedEventArgs e)
        {
            // Not done
            string chapterName = CBChapter.Text;
            string[] episodeString = CBEpisode.Text.Split(' ');
            int episodeNr = -1;
            for (int i = 0; i < episodeString.Length; i++)
            {
                bool succes = int.TryParse(episodeString[i], out episodeNr);
                if (succes)
                {
                    DBQueries.AddMatch(chapterName, episodeNr, (string[])Session.Get("student"));
                    ButtonNavigate(sender, e);
                }
            }
        }

    }
}
