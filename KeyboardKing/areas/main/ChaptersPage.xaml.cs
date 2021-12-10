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

        /// <summary>
        /// <para>Load all chapters and episodes with the highscore of the corresponding user.</para>
        /// This data is used for the listbox in ChaptersPage.xaml
        /// </summary>
        public void LoadAllEpisodes()
        {
            this.Dispatcher.Invoke(() =>
            {
                // Get logged in user data for the GetAllEpisodes query and set it to the itemsource of the overview ListBox.
                UList User = (UList)Session.Get("student");
                List<Episode> Episodes = DBQueries.GetAllEpisodes(User);
                EpOverview.ItemsSource = Episodes;

                // Add a GroupDescription so that the chapters with it's episodes will be split.
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(EpOverview.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("ChapterName");
                view.GroupDescriptions.Add(groupDescription);
                EpOverview.Items.Refresh();
            });
        }

        /// <summary>
        /// <para>Executes method when PlayButton is fired in the EpOverview ListBoxItem.</para>
        /// </summary>
        private void EpOverview_PlayClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.DataContext is Episode)
            {
                //Gets the selected row data
                Episode row = (Episode)button.DataContext;

                //When the episode is finished this event will trigger.
                //Since we can only call Navigate() inside the View this is needed.
                EpisodeController.EpisodeFinished += OnEpisodeFinished;
                Episode episode = EpisodeController.ParseEpisode(row.Id);
                EpisodeController.Initialise(episode);

                NavigationController.NavigateToPage(Pages.EpisodeReadyUpPage);
            }
        }

        private void OnEpisodeFinished(object sender, EventArgs e)
        {
            EpisodeController.EpisodeFinished -= OnEpisodeFinished;
            NavigationController.NavigateToPage(Pages.EpisodeResultPage);
        }
    }
}
