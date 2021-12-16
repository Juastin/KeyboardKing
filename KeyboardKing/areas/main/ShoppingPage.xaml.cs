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
        public ShoppingPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            LoadAllItems();
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        public void LoadAllItems()
        {
            this.Dispatcher.Invoke(() =>
            {
                List<List<string>> Items = new List<List<string>>
                {
                    new List<string> {"Naam1", "/KeyBoardking;component/resources/images/icons/coin.png", "10", "True"},
                    new List<string> {"Naam2", "/KeyBoardking;component/resources/images/icons/coin.png", "100", "True"},
                    new List<string> {"Naam3", "/KeyBoardking;component/resources/images/icons/coin.png", "250", "False"},
                    new List<string> {"Naam4", "/KeyBoardking;component/resources/images/icons/coin.png", "500", "True"},
                    new List<string> {"Naam5", "/KeyBoardking;component/resources/images/hellobeertje_background_4k.png", "1000", "False"},
                    new List<string> {"Naam6", "/KeyBoardking;component/resources/images/icon.png", "10000", "False" }
                };
                LbShop.ItemsSource = Items;
            });
        }
    }
}
