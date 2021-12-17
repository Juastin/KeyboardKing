using KeyboardKing.core;
using Model;
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
using Controller;

namespace KeyboardKing.areas.main
{
    /// <summary>
    /// Interaction logic for FavoritesPage.xaml
    /// </summary>
    public partial class ShoppingPage : JumpPage
    {
        public ShoppingPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            ShopController.Initialize();
            LoadItems(ShopController.GetPageItems());
            UpdateButtonVisibility();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        public void LoadItems(List<Item> items)
        {
            this.Dispatcher.Invoke(() =>
            {
                LbShop.ItemsSource = items;
                LbShop.Items.Refresh();
            });
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ShopController.GetPageItems(ShopController.CurrentPage + 1).Any())
            {
                ShopController.CurrentPage++;
                LoadItems(ShopController.GetPageItems());
                UpdateButtonVisibility();
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (ShopController.CurrentPage - 1 > 0)
            {
                ShopController.CurrentPage--;
                LoadItems(ShopController.GetPageItems());
                UpdateButtonVisibility();
            }
        }

        public void UpdateButtonVisibility()
        {
            PreviousPage.Visibility = ShopController.CurrentPage > 1 ? Visibility.Visible : Visibility.Hidden;
            NextPage.Visibility = ShopController.GetPageItems(ShopController.CurrentPage + 1).Any() ? Visibility.Visible : Visibility.Hidden;
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.DataContext is Item item)
            {
                ShopController.CurrentItem = item;
                MatchController.AddUserInMatchProgress();
                NavigationController.NavigateToPage(Pages.MatchLobbyPage);
            }
        }
    }
}
