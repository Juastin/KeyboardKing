using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Model;
using Controller;
using System.Diagnostics;
using System.Windows.Threading;

namespace KeyboardKing.components
{
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public Pages CurrentPage { get; set; } = Pages.Empty;
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
            
        }
    }

    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Trace.WriteLine(value.ToString());
            Trace.WriteLine(NavigationController.CurrentPage.ToString());
            if (Enum.TryParse(value.ToString(), out Pages page))
                return page == NavigationController.CurrentPage;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
