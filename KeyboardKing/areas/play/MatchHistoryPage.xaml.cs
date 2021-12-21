using Controller;
using DatabaseController;
using KeyboardKing.core;
using Model;
using System.Windows.Data;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchHistoryPage.xaml
    /// </summary>
    public partial class MatchHistoryPage : JumpPage
    {
        public MatchHistoryPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
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
    }
}
