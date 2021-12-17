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
using Model;

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

        public void ShowHandlerDialog()
        {
            Visibility = Visibility.Visible;
            Item item = ShopController.CurrentItem;
            ItemImage.Source = new BitmapImage(new Uri(item.Path, UriKind.Relative));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }
    }
}
