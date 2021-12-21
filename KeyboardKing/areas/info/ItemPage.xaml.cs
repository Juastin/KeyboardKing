using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DatabaseController;
using Model;
using Controller;

namespace KeyboardKing.areas.info
{
    /// <summary>
    /// Interaction logic for ItemPage.xaml
    /// </summary>
    public partial class ItemPage : UserControl
    {
        public delegate void Bought();
        public static event Bought ItemBought;

        public ItemPage()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        public void ShowOverlay()
        {
            Visibility = Visibility.Visible;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            
            if (ShopController.CheckItemExists())
            {
                AudioPlayer.Play(AudioPlayer.Sound.shop_purchase);
                var t = Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(1000);
                    MusicPlayer.PlayNextFrom("shop");
                });

                ShopController.AddItemToPlayer();
            } 
            else
            {
                MessageBox.Show("Het product bestaat niet.");
            }
            ItemBought?.Invoke();
            Visibility = Visibility.Hidden;
        }
    }
}
