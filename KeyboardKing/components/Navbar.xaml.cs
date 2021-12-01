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
        public Pages CurrentPage { get; set; }

        public Navbar()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            switch (CurrentPage)
            {
                case Pages.ChaptersPage:
                    Button_ChaptersPage.Background = Brushes.White;
                    Button_ChaptersPage.Foreground = Brushes.Black;
                    break;
                case Pages.FavoritesPage:
                    Button_FavoritesPage.Background = Brushes.White;
                    Button_FavoritesPage.Foreground = Brushes.Black;
                    break;
                case Pages.MatchOverviewPage:
                    Button_MatchOverviewPage.Background = Brushes.White;
                    Button_MatchOverviewPage.Foreground = Brushes.Black;
                    break;
                case Pages.SettingsPage:
                    Button_SettingsPage.Background = Brushes.White;
                    Button_SettingsPage.Foreground = Brushes.Black;
                    break;
            }
        }
    }
}
