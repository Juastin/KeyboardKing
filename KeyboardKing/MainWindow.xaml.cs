using Controller;
using KeyboardKing.areas.login;
using KeyboardKing.areas.main;
using KeyboardKing.areas.play;
using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboardKing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Frame element that is used to display the pages.
        /// </summary>
        private Frame _mainFrame {get;set;}

        /// <summary>
        /// Used to handle background tasks.
        /// </summary>
        private Timer _timer {get;set;}

        private string _currentPage {get;set;}

        /// <summary>
        /// Dictionary of pages that are used in the app.
        /// </summary>
        private Dictionary<string, JumpPage> _pages {get;set;}

        public MainWindow()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(Tick);
            _timer.Interval = 1000;
            _timer.Start();

            _mainFrame = MainFrame;
            _pages = new()
            {
                // login area
                {"LoginPage", new LoginPage(this)},
                {"RegisterPage", new RegisterPage(this)},
                {"RegisterSkillPage", new RegisterSkillPage(this)},

                // main area
                {"ChaptersPage", new ChaptersPage(this)},
                {"FavoritesPage", new FavoritesPage(this)},
                {"SettingsPage", new SettingsPage(this)},

                // play area
                {"EpisodePage", new EpisodePage(this)},
                {"EpisodeResultPage", new EpisodeResultPage(this)},
                {"MatchLobbyPage", new MatchLobbyPage(this)},
                {"MatchPlayingPage", new MatchPlayingPage(this)},
                {"MatchResultPage", new MatchResultPage(this)},
            };

            // Navigate to the first view.
            Navigate("EpisodePage");
        }

        public void Navigate(string pageName)
        {
            _mainFrame.Navigate(_pages[pageName]);
            _currentPage = pageName;
        }

        public void Navigate(JumpPage oldPage, string pageName)
        {
            oldPage.OnShadow();
            _mainFrame.Navigate(_pages[pageName]);
            _pages[pageName].OnLoad();
            _currentPage = pageName;
        }

        private void Tick(object sender, EventArgs e)
        {
            _pages[_currentPage].OnTick();
        }
    }
}
