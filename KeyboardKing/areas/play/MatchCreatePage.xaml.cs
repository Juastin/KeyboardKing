using KeyboardKing.core;
using Controller;
using System.Windows;
using System.Windows.Data;
using KeyboardKing.data_context;
using DatabaseController;

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

        public override void OnLoad()
        {
            if (MatchController.CheckIfUserExists())
            {
                MessageBox.Show("Je zit al in een match");
                NavigationController.NavigateToPage(Pages.MatchOverviewPage);
            }

            User user = (User)Session.Get("student");
            ListCollectionView lcv = new ListCollectionView(DBQueries.GetAllEpisodes(user));
            lcv.GroupDescriptions.Add(new PropertyGroupDescription("ChapterName"));
            CBEpisode.ItemsSource = lcv;
            CBEpisode.SelectedValuePath = "Id";
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
