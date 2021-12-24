using Controller;
using Model;
using System;
using System.Windows.Controls;
using System.Windows.Media;

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
            NavigationController.ThemeChange += OnThemeChange;
            SetBackgroundColors();
        }

        public void OnThemeChange(object sender, EventArgs e) => SetBackgroundColors();

        public void SetBackgroundColors()
        {
            _backgroundColor = FindResource("Color5") as SolidColorBrush;
            _foregroundColor = FindResource("Color6") as SolidColorBrush;

            switch (CurrentPage)
            {
                case Pages.ChaptersPage:
                    Button_ChaptersPage.Background = _backgroundColor;
                    Button_ChaptersPage.Foreground = _foregroundColor;
                    break;
                case Pages.ShoppingPage:
                    Button_ShoppingPage.Background = _backgroundColor;
                    Button_ShoppingPage.Foreground = _foregroundColor;
                    break;
                case Pages.MatchOverviewPage:
                    Button_MatchOverviewPage.Background = _backgroundColor;
                    Button_MatchOverviewPage.Foreground = _foregroundColor;
                    break;
                case Pages.SettingsPage:
                    Button_SettingsPage.Background = _backgroundColor;
                    Button_SettingsPage.Foreground = _foregroundColor;
                    break;
            }
        }
    }
}
