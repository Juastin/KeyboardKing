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
            string[] User = (string[])Session.Get("student");
            List<List<string>> Episodes = DBQueries.GetAllEpisodes(User);
            int counter = 0;
            while (counter < Episodes.Count)
            {
                chapterEpisode.Items.Add($"{Episodes[counter][0]} - Episode {Episodes[counter][1]} ({Episodes[counter][2]})");
                counter++;
            }
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

    }
}
