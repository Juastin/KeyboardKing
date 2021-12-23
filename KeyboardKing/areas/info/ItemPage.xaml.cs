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
            bool itemExist = ShopController.CheckItemExists();


            // Still needs a check if user has sufficient amount of coins.
            if (ShopController.CheckItemExists())
            {
                if (ShopController.BuyItem())
                {
                    AudioPlayer.Play(AudioPlayer.Sound.shop_purchase);
                    var t = Task.Factory.StartNew(async () =>
                    {
                        await Task.Delay(1000);
                        MusicPlayer.PlayNextFrom("shop");
                    });
                }
                else
                {
                    int insufficientAmount = ShopController.CurrentItem.Price - ((User)Session.Get("student")).Coins;
                    MessageBox.Show($"Je komt {insufficientAmount} coins tekort");
                }
            } 
            else
            {
                MessageBox.Show("Het product bestaat niet.");
            }
            ShopController.UpdateItemsList();
            Visibility = Visibility.Hidden;
        }
    }
}
