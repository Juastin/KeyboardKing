using KeyboardKing.core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Controller;
using Model;
using System.Globalization;

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
                User user = (User)Session.Get("student");
                EpisodeController.RetrieveChapters();
                EpOverview.ItemsSource = EpisodeController.Chapters.SelectMany(c => c.Episodes).ToList();

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
                Episode episode = EpisodeController.ParseEpisode(row.Id);
                EpisodeController.Initialise(episode, false);

                NavigationController.NavigateToPage(Pages.EpisodeReadyUpPage);
            }
        }
    }

    public class ChapterBadgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Chapter chapter = EpisodeController.Chapters.Find(c => c.Name == value.ToString());
            
            return $"/KeyboardKing;component/resources/images/badges/{chapter.Badge}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }
}
