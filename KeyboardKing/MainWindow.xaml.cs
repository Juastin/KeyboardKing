using Controller;
using KeyboardKing.areas.login;
using KeyboardKing.areas.main;
using KeyboardKing.areas.play;
using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Model;
using Model.event_args;
using KeyboardKing.areas.info;
using System.ComponentModel;

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
        private Dictionary<Pages, JumpPage> _pages {get;set;}

        private Dictionary<string, Theme> _themes;

        private ResourceDictionary themeDictionary = Application.Current.Resources.MergedDictionaries[0];

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
                // info area
                {Pages.MessagePage, new MessagePage(this)},

                // login area
                {Pages.LoginPage, new LoginPage(this)},
                {Pages.RegisterPage, new RegisterPage(this)},
                {Pages.RegisterSkillPage, new RegisterSkillPage(this)},

                // main area
                {Pages.ChaptersPage, new ChaptersPage(this)},
                {Pages.FavoritesPage, new FavoritesPage(this)},
                {Pages.SettingsPage, new SettingsPage(this)},

                // play area
                {Pages.EpisodeReadyUpPage, new EpisodeReadyUpPage(this)},
                {Pages.EpisodePage, new EpisodePage(this)},
                {Pages.EpisodeResultPage, new EpisodeResultPage(this)},
                {Pages.MatchOverviewPage, new MatchOverviewPage(this)},
                {Pages.MatchCreatePage, new MatchCreatePage(this)},
                {Pages.MatchLobbyPage, new MatchLobbyPage(this)},
                {Pages.MatchPlayingPage, new MatchPlayingPage(this)},
                {Pages.MatchResultPage, new MatchResultPage(this)},
            };

            _themes = new()
            {   
                {"Light", new Theme("Light Theme", "resources/themes/LightTheme.xaml", "resources/images/kk_background_4K.png")},
                {"Dark", new Theme("Dark Theme", "resources/themes/DarkTheme.xaml", "resources/images/kk_background_dark.png")},
                {"Paint", new Theme("Paint Theme", "resources/themes/PaintTheme.xaml", "resources/images/paint_theme_background.png")},
                {"Space", new Theme("Space Theme", "resources/themes/SpaceTheme.xaml", "resources/images/space_theme_background.png")},
            };

            CBTheme.ItemsSource = _themes;
            CBTheme.SelectedValue = "Space";
            CBTheme.DisplayMemberPath = "Value.ThemeTitle";
            CBTheme.SelectedValuePath = "Key";

            // Navigate to the first view.
            NavigationController.Navigate += OnNavigate;
            NavigationController.NavigateToPage(Pages.LoginPage);
        }
   
        public void OnNavigate(NavigateEventArgs e)
        {

            if (e.OldPage != Pages.Empty)
                _pages[e.OldPage].OnShadow();

            _mainFrame.Navigate(_pages[e.NewPage]);
            _pages[e.NewPage].OnLoad();
        }

        private void Tick(object sender, EventArgs e)
        {
            _pages[NavigationController.CurrentPage].OnTick();
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

        //Switch from theme according to the given paths.
        private void ChangeTheme(Theme theme)
        {
            themeDictionary.Clear();
            themeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = theme.ThemeUri });
            MainBackground.ImageSource = new BitmapImage(theme.BackgroundUri);
        }

        private void CBTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object value = CBTheme.SelectedValue;
            Theme theme;
            if (_themes.TryGetValue((string)value, out theme))
            {
                ChangeTheme(theme);
            }
        }

        //Closes open matches when closing application
        private void CheckBefore_Closing(object sender, CancelEventArgs e)
        {
            if (NavigationController.CurrentPage == Pages.MatchLobbyPage || NavigationController.CurrentPage == Pages.MatchPlayingPage)
            {
                MatchController.RemoveUserInMatchProgress();

                if (!DBQueries.GetMatchProgress((int)Session.Get("matchId")).Any())
                {
                    DBQueries.DeleteMatch((int)Session.Get("matchId"));
                }

            }
        }


    }
}
