using KeyboardKing.core;
using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Controller;
using System.Threading.Tasks;
using System;

namespace KeyboardKing.areas.main
{
    /// <summary>
    /// Interaction logic for FavoritesPage.xaml
    /// </summary>
    public partial class ShoppingPage : JumpPage
    {
        private DateTime _tickCheck {get;set;} = DateTime.MinValue;
        private bool _isSwitching {get;set;} = false;
        private int _lastFetchedByUserId {get;set;} = 0;

        public ShoppingPage(MainWindow w) : base(w)
        {
            InitializeComponent();
            ShopController.ShopDataChanged += OnItemListChanged;
        }

        public override void OnLoad()
        {
            // AUDIO
            if (!_isSwitching)
            {
                AudioPlayer.Play(AudioPlayer.Sound.shop_enter);
                var t = Task.Factory.StartNew(async () =>
                {
                    _isSwitching = true;
                    await Task.Delay(1000);
                    MusicPlayer.PlayNextFrom("shop");
                    _isSwitching = false;
                });
            }


            // FETCH ITEMS
            DateTime now = DateTime.Now;
            if (_tickCheck.AddMinutes(5) < now || _lastFetchedByUserId != ((User)Session.Get("student")).Id)
            {
                _tickCheck = now;
                _lastFetchedByUserId = ((User)Session.Get("student")).Id;
                ShopController.Initialize();
                UpdateShop();
            }
        }

        public override void OnShadow()
        {
            // AUDIO
            if (!_isSwitching)
            {
                AudioPlayer.Play(AudioPlayer.Sound.shop_exit);
                var t = Task.Factory.StartNew(async () =>
                {
                    _isSwitching = true;
                    await Task.Delay(1000);
                    MusicPlayer.PlayNextFrom("menu_music");
                    _isSwitching = false;
                });
            }
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
            ShopController.CurrentPage += page;
            LoadItems(ShopController.GetPageItems());
            UpdateButtonVisibility();
        }

        // Call all methods necessary for proper view without param (update on current page). 
        public void UpdateShop()
        {
            UpdateShop(0);
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

        // Changes the shop back to page 1
        public void ResetPageIndex()
        {
            ShopController.CurrentPage = 1;
            UpdateShop();
        }

        public void OnItemListChanged(object sender, EventArgs e)
        {
            UpdateShop();
        }
    }
}
