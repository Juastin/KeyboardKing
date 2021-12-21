using Controller;
using KeyboardKing.areas.login;
using KeyboardKing.areas.main;
using KeyboardKing.areas.play;
using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                {Pages.ConfirmationPage, new ConfirmationPage(this)},
                {Pages.PausePage, new PausePage(this)},

                // login area
                {Pages.LoginPage, new LoginPage(this)},
                {Pages.RegisterPage, new RegisterPage(this)},
                {Pages.RegisterSkillPage, new RegisterSkillPage(this)},

                // play area
                {Pages.EpisodeReadyUpPage, new EpisodeReadyUpPage(this)},
                {Pages.EpisodePage, new EpisodePage(this)},
                {Pages.EpisodeResultPage, new EpisodeResultPage(this)},
                {Pages.MatchOverviewPage, new MatchOverviewPage(this)},
                {Pages.MatchHistoryPage, new MatchHistoryPage(this)},
                {Pages.MatchHistoryLeaderboardPage, new MatchHistoryLeaderboardPage(this)},
                {Pages.MatchCreatePage, new MatchCreatePage(this)},
                {Pages.MatchLobbyPage, new MatchLobbyPage(this)},
                {Pages.MatchPlayingPage, new MatchPlayingPage(this)},
                {Pages.MatchResultPage, new MatchResultPage(this)},
                {Pages.MatchWaitingResultPage, new MatchWaitingResultPage(this)},

                // main area
                {Pages.ChaptersPage, new ChaptersPage(this)},
                {Pages.ShoppingPage, new ShoppingPage(this)},
                {Pages.SettingsPage, new SettingsPage(this)} // This page should be at the bottom of the list.
            };

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

        //Closes open matches when closing application
        private void CheckBefore_Closing(object sender, CancelEventArgs e)
        {
            if (NavigationController.CurrentPage == Pages.MatchLobbyPage || NavigationController.CurrentPage == Pages.MatchPlayingPage)
            {
                MatchController.RemoveUserInMatchProgress();

                if (!MatchController.GetMatchProgressInfo().Any())
                {
                    MatchController.DeleteMatch();
                }

            }
        }


    }
}
