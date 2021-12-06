using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
using KeyboardKing.core;
using Model;

namespace KeyboardKing.components
{
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public Pages CurrentPage { get; set; } = Pages.ChaptersPage;

        private SolidColorBrush _backgroundColor { get; set; }
        private SolidColorBrush _foregroundColor { get; set; }

        public Navbar()
        {
            InitializeComponent();
            _backgroundColor = FindResource("Color5") as SolidColorBrush;
            _foregroundColor = FindResource("Color6") as SolidColorBrush;
            Loaded += OnLoaded;
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            //switch (CurrentPage)
            //{
            //    case Pages.ChaptersPage:
            //        Button_ChaptersPage.Background = _backgroundColor;
            //        Button_ChaptersPage.Foreground = _foregroundColor;
            //        break;
            //    case Pages.FavoritesPage:
            //        Button_FavoritesPage.Background = _backgroundColor;
            //        Button_FavoritesPage.Foreground = _foregroundColor;
            //        break;
            //    case Pages.MatchOverviewPage:
            //        Button_MatchOverviewPage.Background = _backgroundColor;
            //        Button_MatchOverviewPage.Foreground = _foregroundColor;
            //        break;
            //    case Pages.SettingsPage:
            //        Button_SettingsPage.Background = _backgroundColor;
            //        Button_SettingsPage.Foreground = _foregroundColor;
            //        break;
            //}
        }
    }
}
