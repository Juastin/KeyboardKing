using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Model;

namespace KeyboardKing.components
{
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public static Pages CurrentPage { get; set; } = Pages.ChaptersPage;

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
    public class PageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null) return false;
            //if (value.ToString().Length == 0) return false;
            //Pages page;
            //if (Enum.TryParse(value.ToString(), out page))
            //    return page == Navbar.CurrentPage;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
