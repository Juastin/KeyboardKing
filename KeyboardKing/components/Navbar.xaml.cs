using System;
using System.Collections.Generic;
using System.Globalization;
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
using KeyboardKing.core;
using Model;

namespace KeyboardKing.components
{
    /// <summary>
    /// Interaction logic for Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(
                "CurrentPage", typeof(Model.Page),
                typeof(Navbar)
            );

        public Model.Page CurrentPage
        {
            get { return (Model.Page)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public Brush ChaptersPageBackground { get => CurrentPage == Model.Page.ChaptersPage ? Brushes.White : Brushes.Transparent; }
        public Brush FavoritesPageBackground { get => CurrentPage == Model.Page.FavoritesPage ? Brushes.White : Brushes.Transparent; }
        public Brush MatchOverviewPageBackground { get => CurrentPage == Model.Page.MatchOverviewPage ? Brushes.White : Brushes.Transparent; }
        public Brush SettingsPageBackground { get => CurrentPage == Model.Page.SettingsPage ? Brushes.White : Brushes.Transparent; }

        public Brush ChaptersPageForground { get => CurrentPage == Model.Page.ChaptersPage ? Brushes.Black : Brushes.White; }
        public Brush FavoritesPageForground { get => CurrentPage == Model.Page.FavoritesPage ? Brushes.Black : Brushes.White; }
        public Brush MatchOverviewPageForground { get => CurrentPage == Model.Page.MatchOverviewPage ? Brushes.Black : Brushes.White; }
        public Brush SettingsPageForground { get => CurrentPage == Model.Page.SettingsPage ? Brushes.Black : Brushes.White; }

        public Navbar()
        {
            InitializeComponent();
            //switch (CurrentPage)
            //{
            //    case Pages.ChaptersPage:
            //        Button_ChaptersPage.Background = Brushes.Black;
            //        Button_ChaptersPage.Foreground = Brushes.White;
            //        break;
            //    case Pages.FavoritesPage:
            //        Button_ChaptersPage.Background = Brushes.Black;
            //        Button_ChaptersPage.Foreground = Brushes.White;
            //        break;
            //    case Pages.MatchOverviewPage:
            //        Button_ChaptersPage.Background = Brushes.Black;
            //        Button_ChaptersPage.Foreground = Brushes.White;
            //        break;
            //    case Pages.SettingsPage:
            //        Button_ChaptersPage.Background = Brushes.Black;
            //        Button_ChaptersPage.Foreground = Brushes.White;
            //        break;
            //}
        }
    }
}
