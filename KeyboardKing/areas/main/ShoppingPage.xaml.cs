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
            UpdateShop(0);
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        // Go to next page and update. 
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            UpdateShop(1);
        }

        // Go to previous page and update.  
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            UpdateShop(-1);
        }

        // Show item preview on item click.
        private void Item_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.DataContext is Item item)
            {
                ShopController.SetCurrentItem(item);
                Popup.ShowOverlay();
            }
        }

        // Call all methods necessary for proper view. 
        public void UpdateShop(int page)
        {
            ShopController.UpdatePage(page);
            LoadItems(ShopController.GetPageItems());
            UpdateButtonVisibility();
        }

        // Update ListBox with given list
        public void LoadItems(List<Item> items)
        {
            this.Dispatcher.Invoke(() =>
            {
                LbShop.ItemsSource = items;
                LbShop.Items.Refresh();
            });
        }

        // Updating button visibility according to its current page.
        public void UpdateButtonVisibility()
        {
            PreviousPage.Visibility = ShopController.CurrentPage > 1 ? Visibility.Visible : Visibility.Hidden;
            NextPage.Visibility = ShopController.CurrentPage != ShopController.MaxPage ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
