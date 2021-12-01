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
                {"MatchOverviewPage", new MatchOverviewPage(this)},
                {"MatchCreatePage", new MatchCreatePage(this)},
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

        // Switch between minimized and maximized window with the right values.
        private void SwitchWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        MainBorder.CornerRadius = new System.Windows.CornerRadius(0);
                        Application.Current.MainWindow.BorderThickness = new System.Windows.Thickness(6);
                        Application.Current.MainWindow.WindowState = WindowState.Maximized;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        MainBorder.CornerRadius = new System.Windows.CornerRadius(8);
                        Application.Current.MainWindow.BorderThickness = new System.Windows.Thickness(0);
                        Application.Current.MainWindow.WindowState = WindowState.Normal;
                        break;
                    }
            }
        }

        // Titlebar area MouseDown event
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // If titlebar area is held down and window is maximized, minimize en set titlebar on mouseposition.
            if (e.LeftButton == MouseButtonState.Pressed && Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                // Calculates the position where the application should be, according to mouseposition.
                var point = PointToScreen(e.MouseDevice.GetPosition(this));

                if (point.X <= RestoreBounds.Width / 2)
                    Left = 0;

                else if (point.X >= RestoreBounds.Width)
                    Left = point.X - (RestoreBounds.Width - (this.ActualWidth - point.X));

                else
                    Left = point.X - (RestoreBounds.Width / 2);

                Top = point.Y - (((FrameworkElement)sender).ActualHeight / 2);

                SwitchWindowState();
                DragMove();
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //Titlebar minimize button event
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        //Titlebar minimize button event
        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindowState();
        }

        //Titlebar minimize button event
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
