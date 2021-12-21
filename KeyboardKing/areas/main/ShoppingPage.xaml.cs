﻿using KeyboardKing.core;
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
        private bool isSwitching {get;set;} = false;

        public ShoppingPage(MainWindow w) : base(w)
        {
            InitializeComponent();
            info.ItemPage.ItemBought += OnItemBought;
        }

        public override void OnLoad()
        {
            // FETCH ITEMS
            ShopController.Initialize();
            UpdateShop();

            // AUDIO
            if (!isSwitching)
                AudioPlayer.Play(AudioPlayer.Sound.shop_enter);
                var t = Task.Factory.StartNew(async () =>
                {
                    isSwitching = true;
                    await Task.Delay(1000);
                    MusicPlayer.PlayNextFrom("shop");
                    isSwitching = false;
                });
        }

        public override void OnShadow()
        {
            // AUDIO
            if (!isSwitching)
                AudioPlayer.Play(AudioPlayer.Sound.shop_exit);
                var t = Task.Factory.StartNew(async () =>
                {
                    isSwitching = true;
                    await Task.Delay(1000);
                    MusicPlayer.PlayNextFrom("menu_music");
                    isSwitching = false;
                });
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

        // Call all methods necessary for proper view. 
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

        public void OnItemBought()
        {
            ShopController.Initialize();
            UpdateShop();
        }
    }
}
