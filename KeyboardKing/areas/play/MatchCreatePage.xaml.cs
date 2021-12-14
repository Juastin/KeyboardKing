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
using Model;

// Source: https://stackoverflow.com/questions/561166/binding-a-wpf-combobox-to-a-custom-list

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

        //https://stackoverflow.com/questions/3585017/grouping-items-in-a-combobox
        public override void OnLoad()
        {
            if (MatchController.CheckIfUserExists())
            {
                MessageBox.Show("Je zit al in een match");
                NavigationController.NavigateToPage(Pages.MatchOverviewPage);
            }

            UList user = (UList)Session.Get("student");
            List<List<string>> episodes = DBQueries.GetAllEpisodes(user);
            int counter = 0;
            List<EpisodeData> listData = new List<EpisodeData>();

            while (counter < episodes.Count)
            {
                listData.Add(new EpisodeData { EpisodeId = int.Parse(episodes[counter][3]), EpisodeName = episodes[counter][2], ChapterName = episodes[counter][0] });
                counter++;
            }
            ListCollectionView lcv = new ListCollectionView(listData);
            lcv.GroupDescriptions.Add(new PropertyGroupDescription("ChapterName"));

            CBEpisode.ItemsSource = lcv;
            CBEpisode.SelectedValuePath = "EpisodeId";

            // Saving for style in XAML: <ComboBox ScrollViewer.VerticalScrollBarVisibility="Hidden" Name="CBEpisode" HorizontalAlignment="Center" ItemContainerStyle="{DynamicResource ComboBoxItem}" Template="{DynamicResource ComboBox}" VerticalAlignment="Top" Width="336" Height="42" FontSize="18" Margin="0,90,0,0"/>
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void BCreateMatch(object sender, RoutedEventArgs e)
        {
            if (CBEpisode.SelectedValue != null)
            {
                MatchController.MakeMatch((int)CBEpisode.SelectedValue);
                NavigationController.NavigateToPage(Pages.MatchLobbyPage);
            }
        }

    }
}
