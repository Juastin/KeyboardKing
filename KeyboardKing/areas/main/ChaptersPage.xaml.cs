using KeyboardKing.core;
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
using Controller;
using Model;

namespace KeyboardKing.areas.main
{
    /// <summary>
    /// Interaction logic for ChaptersPage.xaml
    /// </summary>
    public partial class ChaptersPage : JumpPage
    {
        public ChaptersPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            LoadAllEpisodes();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        public void LoadAllEpisodes()
        {
            this.Dispatcher.Invoke(() =>
            {
                List<List<string>> Episodes = DBQueries.GetAllEpisodes();
                EpOverview.ItemsSource = Episodes;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(EpOverview.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("[0]");
                view.GroupDescriptions.Add(groupDescription);
                EpOverview.Items.Refresh();
            });
        }

        //Executes method when PlayButton is fired in the EpOverview ListBox.
        private void EpOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.DataContext is List<string>)
            {
                //Gets the selected row data
                List<string> row = (List<string>)button.DataContext;

                //When the episode is finished this event will trigger.
                //Since we can only call Navigate() inside the View this is needed.
                EpisodeController.EpisodeFinished += OnEpisodeFinished;
                Episode episode = EpisodeController.ParseEpisode(row[3]);
                EpisodeController.Initialise(episode);

                Navigate("EpisodePage");
            }
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
        {
            EpisodeController.EpisodeFinished -= OnEpisodeFinished;
            Navigate("EpisodeResultPage");
        }
    }
}
