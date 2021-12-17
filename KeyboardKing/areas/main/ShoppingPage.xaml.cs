using KeyboardKing.core;
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

namespace KeyboardKing.areas.main
{
    /// <summary>
    /// Interaction logic for FavoritesPage.xaml
    /// </summary>
    public partial class ShoppingPage : JumpPage
    {
        private static int _currentPage;
        private static int _itemsPerPage;
        private List<List<string>> _items;

        public ShoppingPage(MainWindow w) : base(w)
        {
            InitializeComponent();
            _itemsPerPage = 8;
        }

        public override void OnLoad()
        {
            _currentPage = 1;
            _items = GetAllItems();
            LoadItems(GetPageItems(_items, _currentPage));
            UpdateButtonVisibility();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        public List<List<string>> GetAllItems()
        {
            List<List<string>> Items = new List<List<string>>();
            this.Dispatcher.Invoke(() =>
            {
                Items = new List<List<string>> {
                    new List<string> {"KeyboardKing Light", "/KeyBoardking;component/resources/images/kk_background_4K.png", "10", "True" },
                    new List<string> {"KeyboardKing Dark", "/KeyBoardking;component/resources/images/kk_background_dark.png", "20", "True" },
                    new List<string> {"Space", "/KeyBoardking;component/resources/images/space_theme_background.png", "50", "True"},
                    new List<string> {"Chinese", "/KeyBoardking;component/resources/images/red_chinese_background.png", "50", "True"},
                    new List<string> {"Paint", "/KeyBoardking;component/resources/images/paint_theme_background.png", "100", "False"},
                    new List<string> {"Obsidian", "/KeyBoardking;component/resources/images/obsidian_theme_background.png", "250", "True"},
                    new List<string> {"Hello Beertje", "/KeyBoardking;component/resources/images/hellobeertje_background_4k.png", "500", "False"},
                    new List<string> {"Christmas", "/KeyBoardking;component/resources/images/kk_background_christmas.png", "1000", "False" },
                    new List<string> {"A Shelf on a shelf", "/KeyBoardking;component/resources/images/shopping_shelf.png", "5000", "False" },
                    new List<string> {"A Coin", "/KeyBoardking;component/resources/images/icons/coin.png", "10000", "False" },
                };
            });

            return Items;
        }

        public List<List<string>> GetPageItems(List<List<string>> items, int page)
        {
            var query = (from pageItems in items
                         select pageItems)
                         .Skip(_itemsPerPage * (page - 1))
                         .Take(_itemsPerPage);

            return query.ToList();
        }

        public void LoadItems(List<List<string>> items)
        {
            this.Dispatcher.Invoke(() =>
            {
                LbShop.ItemsSource = items;
                LbShop.Items.Refresh();
            });
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (GetPageItems(_items, _currentPage + 1).Any())
            {
                _currentPage++;
                LoadItems(GetPageItems(_items, _currentPage));
                UpdateButtonVisibility();
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage - 1 > 0)
            {
                _currentPage--;
                List<List<string>> Items = GetAllItems();
                LoadItems(GetPageItems(Items, _currentPage));
                UpdateButtonVisibility();
            }
        }

        public void UpdateButtonVisibility()
        {
            PreviousPage.Visibility = _currentPage > 1 ? Visibility.Visible : Visibility.Hidden;
            NextPage.Visibility = GetPageItems(_items, _currentPage + 1).Any() ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
