using KeyboardKing.areas.login;
using KeyboardKing.areas.main;
using KeyboardKing.areas.play;
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
        /// Dictionary of pages that are used in the app.
        /// </summary>
        private Dictionary<string, JumpPage> _pages {get;set;}

        public MainWindow()
        {
            InitializeComponent();

            _mainFrame = MainFrame;
            _pages = new()
            {
                // login area
                {"LoginPage", new LoginPage(this)},
                {"RegisterPage", new RegisterPage(this)},
                {"RegisterPage2", new RegisterPage2(this)},

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
            Navigate("LoginPage");
        }

        public void Navigate(string pageName)
        {
            _mainFrame.Navigate(_pages[pageName]);
        }
    }
}
